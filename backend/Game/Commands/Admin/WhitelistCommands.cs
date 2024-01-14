using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models.Account;
using Database.Models.Inventory;
using Database.Models;
using Database.Services;
using Newtonsoft.Json;
using AltV.Net;

namespace Game.Commands.Admin
{
	public static class WhitelistCommands
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, string, string>("Server:Whitelist:GetDiscordId", GetDiscordId);
			Alt.OnClient<RPPlayer, string, string, string>("Server:Whitelist:Finish", Finish);
		}

		[Command("whitelistplayer")]
		public static void WhitelistPlayer(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.GUIDE) return;

			if (AccountService.IsNameTaken(targetName))
			{
				player.Notify("Whitelist", "Der angegebene Name ist bereits vergeben!", Core.Enums.NotificationType.ERROR);
				return;
			}

			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Whitelist",
				Message = "Gebe die Forum ID der Person ein.",
				Type = (int)InputType.TEXT,
				CallbackEvent = "Server:Whitelist:GetDiscordId",
				CallbackArgs = new List<object>() { targetName }
			}));
		}

		private static void GetDiscordId(RPPlayer player, string name, string forumIdString)
		{
			if (player.AdminRank < AdminRank.GUIDE) return;

			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Whitelist",
				Message = "Gebe die Discord ID der Person ein.",
				Type = (int)InputType.TEXT,
				CallbackEvent = "Server:Whitelist:Finish",
				CallbackArgs = new List<object>() { name, forumIdString }
			}));
		}

		private static void Finish(RPPlayer player, string name, string forumIdString, string discordIdString)
		{
			if (player.AdminRank < AdminRank.GUIDE) return;

			if(!int.TryParse(forumIdString, out var forumId) || AccountService.IsForumIdTaken(forumId))
			{
				player.Notify("Whitelist", "Die angegebene Forum ID ist ungültig!", NotificationType.ERROR);
				return;
			}

			if (!ulong.TryParse(discordIdString, out var discordId) || AccountService.IsDiscordIdTaken(discordId))
			{
				player.Notify("Whitelist", "Die angegebene Discord ID ist ungültig!", NotificationType.ERROR);
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
			var teamLocker = new InventoryModel(8, 100f, InventoryType.TEAM_LOCKER);
			InventoryService.Add(inv, labInput, labOutput, locker, teamLocker);

			var license = new LicenseModel();
			LicenseService.Add(license);

			var account = new AccountModel(
				name,
				string.Empty,
				0,
				0,
				0,
				0,
				0,
				5000,
				40000,
				AccountService.GenerateUniquePhoneNumber(),
				pos.Id,
				custom.Id,
				clothes.Id,
				inv.Id,
				labInput.Id,
				labOutput.Id,
				locker.Id,
				0,
				license.Id,
				false,
				teamLocker.Id);
			AccountService.Add(account);
			player.Notify("Whitelist", "Der Account wurde erfolgreich erstellt!", NotificationType.SUCCESS);
		}
	}
}
