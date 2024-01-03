using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models.Account;
using Database.Services;
using Game.Controllers;
using Logs;
using Newtonsoft.Json;

namespace Game.Modules
{
    public static class LoginModule
	{ //private static Regex Regex { get; } = new("[A-Za-z]+_+[A-Za-z]", RegexOptions.IgnoreCase);
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, string>("Server:Login:Kick", Kick);
			Alt.OnClient<RPPlayer, string, int>("Server:Login:Auth", Auth);
		}

		private static void Kick(RPPlayer player, string reason)
		{
			player.Kick(reason);
		}

		[Core.Attribute.ServerEvent(ServerEventType.PLAYER_CONNECT)]
		public static void OnPlayerConnect(RPPlayer player)
		{
			var name = player.Name.ToLower();
			if(RPPlayer.All.Any(x => x.Name.ToLower() == name))
			{
				player.Kick("Es ist ein Fehler aufgetreten!");
				return;
			}

			player.SetDimension(10000 + (int)player.Id);
		}

		private static void Auth(RPPlayer player, string oAuthToken, int localIdentifier)
		{
			if (player.LoggedIn) return;

			if(oAuthToken == string.Empty)
			{
				player.Kick("Authentication failed! (Code: 2)");
				return;
			}

			// get the discord oauth data
			var http = new HttpClient();
			http.DefaultRequestHeaders.Add("Authorization", $"Bearer {oAuthToken}");

			var response = http.GetAsync("https://discordapp.com/api/users/@me").Result;

			if(!response.IsSuccessStatusCode)
			{
				player.Kick("Authentication failed! (Code: 3)");
				return;
			}

			var responseData = response.Content.ReadAsStringAsync().Result;
			var data = JsonConvert.DeserializeObject<dynamic>(responseData);
			if(data == null || data?.id == null)
			{
				player.Kick("Authentication failed! (Code: 4)");
				return;
			}

			var discordId = Convert.ToInt64((string)data!.id);

			// set default data
			player.Model = 1885233650;
			player.Spawn(new(0, 0, 72), 0);
			player.SetInvincible(false);
			player.SetStreamSyncedMetaData("ALIVE", true);
			player.SetStreamSyncedMetaData("CUFFED", false);
			player.SetStreamSyncedMetaData("ROPED", false);
			player.SetStreamSyncedMetaData("STABILIZED", false);

			var account = AccountService.Get(player.Name);
			if (account == null)
			{
				player.Kick($"Es konnte kein Account mit dem Name {player.Name} gefunden werden! Du kannst einen Account im Forum unter https://pegasusrp.de/ erstellen.");
				return;
			}

			if(RPPlayer.All.Any(x => x.DbId == account.Id))
			{
				player.Kick("Es ist ein Fehler aufgetreten!");
				return;
			}

			// first connect multiaccount check
			if (account.LastOnline < DateTime.Now.AddYears(-10) && AccountService.HasMulti(player.SocialClubId, discordId))
			{
				account.BannedUntil = DateTime.Now.AddYears(10);
				account.BanReason = "Multiaccount";
				AccountService.Update(account);
				player.Kick("Du wurdest gebannt! Grund: Multiaccount");
				return;
			}

			// whitelist check
			/*if (!account.Whitelisted)
			{
				player.Kick("Du bist nicht whitelisted!");
				return;
			} */

			// identifier check
			if (CheckUserCredentials(player, account) || (account.DiscordId > 0 && discordId != account.DiscordId) || (localIdentifier > 0 && localIdentifier != account.Id))
			{
				player.Kick("Bitte melde dich im Support! (Identifier mismatch)");
				return;
			}

			// ban check
			if (account.BannedUntil > DateTime.Now)
			{
				player.Kick($"Du bist noch bis zum {account.BannedUntil:dd.MM.yyyy} vom Gameserver gesperrt!");
				return;
			}

			UpdateUserData(player, discordId, account);
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
			account.PhoneVolume = player.PhoneVolume;
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

			VoiceModule.OnPlayerDisconnect(player);

			foreach(var target in RPPlayer.All.ToList())
			{
				if (target.Dimension != player.Dimension || target.Position.Distance(player.Position) > 50) continue;

				target.Notify("Anti Offlineflucht", $"{player.Name} hat das Spiel verlassen.", NotificationType.INFO);
			}

			LogService.LogPlayerConnection(account.Id, false);
		}

		private static void Login(RPPlayer player, AccountModel account)
		{
			if (account.BanOnConnect)
			{
				account.BannedUntil = DateTime.Now.AddYears(10);
				account.BanReason = "Multiaccount";
				AccountService.Update(account);
				player.Kick("Du wurdest gebannt! Grund: Multiaccount");
				return;
			}

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

		private static bool CheckUserCredentials(RPPlayer player, AccountModel account)
		{
			bool mismatch = false;

			if (account.SocialclubId > 0 && player.SocialClubId != account.SocialclubId)
			{
				mismatch = true;
			}

			if (account.HardwareId > 0 && player.HardwareIdHash != account.HardwareId)
			{
				mismatch = true;
			}

			if (account.HardwareIdEx > 0 && player.HardwareIdExHash != account.HardwareIdEx)
			{
				mismatch = true;
			}

			return mismatch;
		}

		private static void UpdateUserData(RPPlayer player, long discordId, AccountModel account)
		{
			if (account.SocialclubId != 0 && account.HardwareId != 0 && account.HardwareId != 0 && account.HardwareIdEx != 0 && account.DiscordId != 0) return;

			account.SocialclubId = player.SocialClubId;
			account.HardwareId = player.HardwareIdHash;
			account.HardwareIdEx = player.HardwareIdExHash;
			account.DiscordId = discordId;
			account.LastOnline = DateTime.Now;
			AccountService.Update(account);
		}
	}
}