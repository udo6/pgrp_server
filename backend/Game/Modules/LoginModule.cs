using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Account;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;
using Game.Streamer;
using Logs;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Game.Modules
{
    public static class LoginModule
	{
		private static Regex Regex { get; } = new("[A-Za-z]+_+[A-Za-z]", RegexOptions.IgnoreCase);

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:Login:Auth", Auth);
		}

		private static void Auth(RPPlayer player)
		{
			if (player.LoggedIn) return;

			player.Model = 1885233650;
			player.Spawn(new(0, 0, 72), 0);
			player.SetInvincible(false);
			player.SetDimension((int)player.Id);
			player.SetStreamSyncedMetaData("ALIVE", true);
			player.SetStreamSyncedMetaData("CUFFED", false);
			player.SetStreamSyncedMetaData("ROPED", false);
			player.SetStreamSyncedMetaData("STABILIZED", false);

			player.Emit("Client:MarkerStreamer:SetMarkers", JsonConvert.SerializeObject(MarkerStreamer.Markers));

			var account = AccountService.Get(player.Name);
			if (account == null)
			{
				RegisterPlayer(player, false);
				player.Kick("Du bist nicht whitelisted!");
				return;
			}

			if (!account.Whitelisted)
			{
				player.Kick("Du bist nicht whitelisted!");
				return;
			}

			if (CheckUserCredentials(player, account))
			{
				player.Kick("Authentication failed!");
				return;
			}

			if (account.BannedUntil > DateTime.Now)
			{
				player.Kick($"Du bist noch bis zum {account.BannedUntil:dd.MM.yyyy} vom Gameserver gesperrt! Grund: {account.BanReason}");
				return;
			}

			if (account.DiscordId != 0 && player.DiscordId != 0 && player.DiscordId != account.DiscordId)
			{
				// Todo: add logs
			}

			UpdateUserData(player, account);
			Login(player, account);
		}

		[Core.Attribute.ServerEvent(ServerEventType.PLAYER_DISCONNECT)]
		public static void OnPlayerDisconnect(RPPlayer player, string reason)
		{
			RPPlayer.All.Remove(player);

			if (!player.LoggedIn) return;

			if (player.SWATDuty)
				PoliceModule.SetSWATDuty(player, false);

			var account = AccountService.Get(player.DbId);
			if(account == null) return;

			account.Health = player.Health;
			account.Armor = player.Armor;
			account.ArmorItemId = player.VestItemId;
			account.Hunger = player.Hunger;
			account.Thirst = player.Thirst;
			account.Alive = player.Alive;
			account.Coma = player.Coma;
			account.Stabilized = player.Stabilized;
			account.InjuryType = player.InjuryType;
			account.Jailtime = player.Jailtime;
			account.Level = player.Level;
			account.Xp = player.Xp;
			account.XpTicks = player.XpTicks;
			account.IsInHospital = player.IsInHospital;
			AccountService.Update(account);

			var pos = PositionService.Get(player.PositionId);
			if(pos == null) return;

			if (player.InInterior)
			{
				pos.Position = player.OutsideInteriorPosition;
			}
			else
			{
				pos.Position = player.Position;
				pos.Rotation = player.Rotation;
			}

			PositionService.Update(pos);

			foreach(var target in RPPlayer.All.ToList())
			{
				if (target.Dimension != player.Dimension || target.Position.Distance(player.Position) > 50) continue;

				target.Notify("Anti Offlineflucht", $"{player.Name} hat das Spiel verlassen.", NotificationType.INFO);
			}

			LogService.LogPlayerConnection(account.Id, false);
		}

		private static void Login(RPPlayer player, AccountModel account)
		{
			ApplyPlayerIds(player, account);

			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			LogService.LogPlayerConnection(account.Id, true);

			if (!custom.Finished)
			{
				CreatorModule.SendToCreator(player, custom, new(-1042.4572f, -2745.323f, 21.343628f));
				return;
			}

			PlayerController.LoadPlayer(player);
		}

		private static void ApplyPlayerIds(RPPlayer player, AccountModel account)
		{
			player.DbId = account.Id;
			player.CustomizationId = account.CustomizationId;
			player.ClothesId = account.ClothesId;
			player.InventoryId = account.InventoryId;
			player.LaboratoryInputInventoryId = account.LaboratoryInputInventoryId;
			player.LaboratoryOutputInventoryId = account.LaboratoryOutputInventoryId;
			player.LockerInventoryId = account.LockerInventoryId;
			player.PositionId = account.PositionId;
			player.TeamId = account.TeamId;
			player.BusinessId = account.BusinessId;
			player.LicenseId = account.LicenseId;
		}

		private static void RegisterPlayer(RPPlayer player, bool login)
		{
			if(!Regex.IsMatch(player.Name))
			{
				player.Kick("Der angegebene Name entspricht nicht dem Format Vorname_Nachname! (alt:V Einstellungen)");
				return;
			}

			var multi = AccountService.Get(player.SocialClubId, player.HardwareIdHash, player.HardwareIdExHash, player.DiscordId);
			if(multi != null)
			{
				player.Kick($"Du hast bereits einen Account! ({multi.Name})");
				return;
			}

			var pos = new PositionModel(-1042.4572f, -2745.323f, 21.343628f, 0, 0, -0.49473903f);
			PositionService.Add(pos);

			var custom = new CustomizationModel();
			CustomizationService.Add(custom);

			var clothes = new ClothesModel();
			ClothesService.Add(clothes);

			var inv = new InventoryModel(6, 25f, InventoryType.PLAYER);
			var labInput = new InventoryModel(8, 30f, InventoryType.LAB_INPUT);
			var labOutput = new InventoryModel(8, 60f, InventoryType.LAB_OUTPUT);
			var locker = new InventoryModel(8, 100f, InventoryType.LOCKER);
			InventoryService.Add(inv, labInput, labOutput, locker);

			var license = new LicenseModel();
			LicenseService.Add(license);

			var account = new AccountModel(
				player.Name,
				player.SocialClubId,
				player.HardwareIdHash,
				player.HardwareIdExHash,
				player.DiscordId,
				5000,
				45000,
				AccountService.GenerateUniquePhoneNumber(),
				pos.Id,
				custom.Id,
				clothes.Id,
				inv.Id,
				labInput.Id,
				labOutput.Id,
				locker.Id,
				0,
				license.Id);
			AccountService.Add(account);

			if(login) Login(player, account);
		}

		private static bool CheckUserCredentials(RPPlayer player, AccountModel account)
		{
			bool mismatch = false;

			if (account.SocialclubId != 0 && player.SocialClubId != account.SocialclubId)
			{
				mismatch = true;
			}

			if (account.HardwareId != 0 && player.HardwareIdHash != account.HardwareId)
			{
				mismatch = true;
			}

			if (account.HardwareIdEx != 0 && player.HardwareIdExHash != account.HardwareIdEx)
			{
				mismatch = true;
			}

			return mismatch;
		}

		private static void UpdateUserData(RPPlayer player, AccountModel account)
		{
			if (account.SocialclubId != 0 && account.HardwareId != 0 && account.HardwareId != 0 && account.HardwareIdEx != 0 && account.DiscordId != 0) return;

			account.SocialclubId = player.SocialClubId;
			account.HardwareId = player.HardwareIdHash;
			account.HardwareIdEx = player.HardwareIdExHash;
			account.DiscordId = player.DiscordId;
			account.LastOnline = DateTime.Now;
			AccountService.Update(account);
		}
	}
}