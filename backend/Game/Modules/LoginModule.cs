using AltV.Net;
using AltV.Net.Data;
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
	{
		private static Random Random = new();

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, string>("Server:Login:PlayerLoaded", PlayerLoaded);
			Alt.OnClient<RPPlayer, string>("Server:Login:Kick", Kick);
			Alt.OnClient<RPPlayer, string>("Server:Login:Auth", Auth);
		}

		private static void PlayerLoaded(RPPlayer player, string token)
		{
			if (player.LoggedIn) return;

			var pos = new Position(-578.3736f, -718.2198f, 132.77148f);

			player.Model = 1885233650;
			player.Spawn(pos, 0);
			player.SetPosition(pos);
			player.Invincible = false;
			player.SetStreamSyncedMetaData("ALIVE", true);
			player.SetStreamSyncedMetaData("CUFFED", false);
			player.SetStreamSyncedMetaData("ROPED", false);
			player.SetStreamSyncedMetaData("STABILIZED", false);

			var discordId = GetDiscordId(token);
			if(discordId == 0)
			{
				player.Kick("Du wurdest gekicked! Grund: Discord authentifizierung fehlgeschlagen!");
				return;
			}

			player.OAuthDiscordId = discordId;
			player.AuthCode = GenerateAuthCode();
			Discord.Main.SendAuthCode(discordId, player.AuthCode);
			player.ShowComponent("Login", true, player.Name);
		}

		private static void Kick(RPPlayer player, string reason)
		{
			player.Kick(reason);
		}

		[Core.Attribute.ServerEvent(ServerEventType.PLAYER_CONNECT)]
		public static void OnPlayerConnect(RPPlayer player, string reason)
		{
			var name = player.Name.ToLower();
			if(RPPlayer.All.Any(x => x.LoggedIn && x.Name.ToLower() == name))
			{
				player.Kick("Du wurdest gekicked! Grund: Dieser Account ist bereits eingeloggt!");
				return;
			}

			player.SetDimension(100000 + (int)player.Id);
		}

		private static void Auth(RPPlayer player, string code)
		{
			if (player.LoggedIn || player.OAuthDiscordId == 0) return;

			if(player.AuthCode != code)
			{
				if(player.AuthTries > 3)
				{
					player.Kick("Du wurdest gekicked! Grund: Anmeldung fehlgeschlagen! Bitte starte dein Spiel neu und versuche es erneut.");
					return;
				}

				player.AuthTries++;
				player.Notify("Information", $"Der angegebene Code stimmt nicht überein!", NotificationType.ERROR);
				return;
			}

			var account = AccountService.Get(player.Name);
			if (account == null)
			{
				player.Kick($"Es konnte kein Account mit dem Name {player.Name} gefunden werden! Du kannst einen Account im Forum unter https://pegasusrp.de/ erstellen.");
				return;
			}

			if(RPPlayer.All.Any(x => x.DbId == account.Id))
			{
				player.Kick("Du wurdest gekicked! Grund: Dieser Account ist bereits eingeloggt!");
				return;
			}

			// first connect multiaccount check
			if (account.LastOnline < DateTime.Now.AddYears(-10) && AccountService.HasMulti(player.SocialClubId, player.OAuthDiscordId))
			{
				account.SocialclubId = player.SocialClubId;
				account.HardwareId = player.HardwareIdExHash;
				account.DiscordId = player.OAuthDiscordId;
				account.IP = player.Ip;
				account.BannedUntil = DateTime.Now.AddYears(10);
				account.BanReason = "Multiaccount";
				AccountService.Update(account);
				player.Kick("Du wurdest gebannt! Grund: Multiaccount");
				return;
			}

			var anyBannedAcc = AccountService.AnyBannedAccounts(player.Ip, player.SocialClubId, player.OAuthDiscordId, player.HardwareIdHash, account.UseHardwareId, player.HardwareIdExHash, account.UseHardwareIdEx);
			if (anyBannedAcc != null)
			{
				player.Kick($"Du wurdest gekicked! Grund: Bitte melde dich im Support! Code: 37 ({anyBannedAcc.Id})");
				return;
			}

			/*if (!account.Whitelisted)
			{
				player.Kick("Du wurdest gekicked! Grund: Du bist nicht whitelisted!");
				return;
			}*/

			if (CheckUserCredentials(player, account) || (account.DiscordId > 0 && player.OAuthDiscordId != account.DiscordId))
			{
				player.Kick("Du wurdest gekicked! Grund: Bitte melde dich im Support! (Identifier mismatch)");
				return;
			}

			UpdateUserData(player, player.OAuthDiscordId, account);

			// ban check
			if (account.BannedUntil > DateTime.Now)
			{
				player.Kick($"Du wurdest gekicked! Grund: Du bist noch bis zum {account.BannedUntil:dd.MM.yyyy} vom Gameserver gesperrt!");
				return;
			}

			player.ShowComponent("Login", false);
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

		private static string GenerateAuthCode()
		{
			var length = 32;
			var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";
			var result = "";

			for (int i = 0; i < length; i++)
			{
				var random = Random.Next(0, chars.Length);
				result += chars[random];
			}

			return result;
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

		private static void UpdateUserData(RPPlayer player, ulong discordId, AccountModel account)
		{
			if (account.SocialclubId != 0 && account.HardwareId != 0 && account.HardwareId != 0 && account.HardwareIdEx != 0 && account.DiscordId != 0) return;

			account.IP = player.Ip;
			account.SocialclubId = player.SocialClubId;
			account.HardwareId = player.HardwareIdHash;
			account.HardwareIdEx = player.HardwareIdExHash;
			account.DiscordId = discordId;
			account.LastOnline = DateTime.Now;
			AccountService.Update(account);
		}

		private static ulong GetDiscordId(string token)
		{
			if (token == string.Empty) return 0;

			var http = new HttpClient();
			http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

			var response = http.GetAsync("https://discordapp.com/api/users/@me").Result;

			if (!response.IsSuccessStatusCode) return 0;

			var responseData = response.Content.ReadAsStringAsync().Result;
			var data = JsonConvert.DeserializeObject<dynamic>(responseData);
			if (data == null || data?.id == null) return 0;

			return Convert.ToUInt64((string)data!.id);
		}
	}
}