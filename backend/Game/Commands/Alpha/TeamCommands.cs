using AltV.Net.Enums;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Alpha
{
	public static class TeamCommands
	{
		private static List<int> TeamIds = new()
		{
			8, // MG13
			6, // LCN
			7, // YKZ
			10, // YKZ
		};

		private static List<int> TeamClothesIds = new()
		{
			102, // MG13
			104, // LCN
			106, // YKZ
			107, // MNC
		};

		[Command("team")]
		public static void SetTeamAlpha(RPPlayer player, int index)
		{
			if (index >= TeamIds.Count || index < 0) return;

			var clothesId = TeamClothesIds[index];

			PlayerController.ApplyPlayerCustomization(player);

			var teamClothes = ClothesService.Get(clothesId);
			if (teamClothes == null) return;

			var clothes = ClothesService.Get(player.ClothesId);
			if (clothes == null) return;

			clothes.Mask = teamClothes.Mask;
			clothes.MaskColor = teamClothes.MaskColor;
			clothes.MaskDlc = teamClothes.MaskDlc;
			clothes.Top = teamClothes.Top;
			clothes.TopColor = teamClothes.TopColor;
			clothes.TopDlc = teamClothes.TopDlc;
			clothes.Body = teamClothes.Body;
			clothes.BodyColor = teamClothes.BodyColor;
			clothes.BodyDlc = teamClothes.BodyDlc;
			clothes.Undershirt = teamClothes.Undershirt;
			clothes.UndershirtColor = teamClothes.UndershirtColor;
			clothes.UndershirtDlc = teamClothes.UndershirtDlc;
			clothes.Pants = teamClothes.Pants;
			clothes.PantsColor = teamClothes.PantsColor;
			clothes.PantsDlc = teamClothes.PantsDlc;
			clothes.Shoes = teamClothes.Shoes;
			clothes.ShoesColor = teamClothes.ShoesColor;
			clothes.ShoesDlc = teamClothes.ShoesDlc;
			clothes.Accessories = teamClothes.Accessories;
			clothes.AccessoriesColor = teamClothes.AccessoriesColor;
			clothes.AccessoriesDlc = teamClothes.AccessoriesDlc;
			clothes.Armor = teamClothes.Armor;
			clothes.ArmorColor = teamClothes.ArmorColor;
			clothes.ArmorDlc = teamClothes.ArmorDlc;
			clothes.Decals = teamClothes.Decals;
			clothes.DecalsColor = teamClothes.DecalsColor;
			clothes.DecalsDlc = teamClothes.DecalsDlc;
			clothes.Hat = teamClothes.Hat;
			clothes.HatColor = teamClothes.HatColor;
			clothes.HatDlc = teamClothes.HatDlc;
			clothes.Glasses = teamClothes.Glasses;
			clothes.GlassesColor = teamClothes.GlassesColor;
			clothes.GlassesDlc = teamClothes.GlassesDlc;
			clothes.Ears = teamClothes.Ears;
			clothes.EarsColor = teamClothes.EarsColor;
			clothes.EarsDlc = teamClothes.EarsDlc;
			clothes.Watch = teamClothes.Watch;
			clothes.WatchColor = teamClothes.WatchColor;
			clothes.WatchDlc = teamClothes.WatchDlc;
			clothes.Bracelet = teamClothes.Bracelet;
			clothes.BraceletColor = teamClothes.BraceletColor;
			clothes.BraceletDlc = teamClothes.BraceletDlc;

			ClothesService.Update(clothes);
			PlayerController.ApplyPlayerClothes(player);

			LoadoutService.ClearPlayerLoadout(player.DbId);
			player.RemoveAllWeapons(true);
			player.Weapons.Clear();

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			inventory.Slots = 16;
			inventory.MaxWeight = 60f;
			InventoryService.Update(inventory);

			InventoryService.ClearInventoryItems(inventory.Id);
			InventoryController.AddItem(player.InventoryId, 10, 10);
			InventoryController.AddItem(player.InventoryId, 9, 10);
			InventoryController.AddItem(player.InventoryId, 8, 5);
			InventoryController.AddItem(player.InventoryId, 58, 1);
			InventoryController.AddItem(player.InventoryId, 117, 50);
			InventoryController.AddItem(player.InventoryId, 12, 1);

			var teamId = TeamIds[index];
			TeamController.SetPlayerTeam(player, teamId, 0, false, false, false);
			var team = TeamService.Get(teamId);
			if (team == null) return;

			var teamPos = PositionService.Get(team.PositionId);
			if (teamPos == null) return;

			player.SetPosition(teamPos.Position);
		}

		[Command("bringteam")]
		public static void BringTeam(RPPlayer player, int team)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			foreach(var target in RPPlayer.All.ToList())
			{
				if (target.TeamId != team) continue;

				target.SetPosition(player.Position);
				target.Notify("Information", $"Du wurdest von {player.Name} teleportiert!", Core.Enums.NotificationType.INFO);
			}

			player.Notify("Information", $"Du hast die Fraktion {team} zu dir teleportiert!", Core.Enums.NotificationType.INFO);
		}

		[Command("reviveteam")]
		public static void ReviveTeam(RPPlayer player, int team)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			foreach (var target in RPPlayer.All.ToList())
			{
				if (target.TeamId != team) continue;

				PlayerController.SetPlayerAlive(target, false);
				target.Notify("Information", $"Du wurdest von {player.Name} wiederbelebt!", Core.Enums.NotificationType.INFO);
			}

			player.Notify("Information", $"Du hast die Fraktion {team} wiederbelebt!", Core.Enums.NotificationType.INFO);
		}
	}
}
