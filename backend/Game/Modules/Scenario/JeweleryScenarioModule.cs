using AltV.Net;
using AltV.Net.Data;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.Modules.Scenario
{
	public static class JeweleryScenarioModule
	{
		public static bool IsBeingAttacked = false;
		public static bool HasBeenAttacked = false;
		public static readonly Position HackPosition = new(-630.9363f, -229.59561f, 38.041748f);
		public static readonly Position LootInventoryPosition = new(-619.63513f, -228.94945f, 38.041748f);
		public static readonly int LootInventoryId = 2;
		public static readonly int LootItemId = 280;

		[Initialize]
		public static void Initialize()
		{
			var loot = (RPShape)Alt.CreateColShapeCylinder(LootInventoryPosition.Down(), 2f, 2f);
			loot.ShapeId = 1;
			loot.ShapeType = ColshapeType.JEWELERY_LOOT;
			loot.Size = 2f;
			loot.InventoryId = LootInventoryId;
		}

		public static void StartHacking(RPPlayer player)
		{
			if (player.IsInVehicle) return;

			if (HasBeenAttacked)
			{
				player.Notify("Information", "Der Juwelier wurde bereits ausgeraubt!", NotificationType.ERROR);
				return;
			}

			if (FederalBankScenarioModule.IsBeingAttacked)
			{
				player.Notify("Information", "Es wird bereits ein Objekt ausgeraubt!", NotificationType.ERROR);
				return;
			}

			TeamController.Broadcast(player.TeamId, "Deine Fraktion fängt an den Juwelier zu hacken!", NotificationType.WARN);
			TeamController.Broadcast(new List<int>() { 1, 2 }, "Es wurde ein Alarm im Juwelier gemeldet!", NotificationType.WARN);

			IsBeingAttacked = true;

			player.PlayAnimation(AnimationType.WELDING);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;
				FinishHacking(player);
			}, 1200000, () =>
			{
				IsBeingAttacked = false;
			});
		}

		public static void FinishHacking(RPPlayer player)
		{
			if (HasBeenAttacked) return;

			IsBeingAttacked = false;

			var jumppoint = JumppointService.Get(23);
			if (jumppoint == null) return;

			jumppoint.Locked = false;
			JumppointService.Update(jumppoint);

			InventoryService.ClearInventoryItems(LootInventoryId);
			InventoryService.AddInventoryItems(
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 1, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 2, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 3, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 4, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 5, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 6, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 7, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 8, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 9, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 10, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 11, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 12, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 13, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 14, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 15, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 16, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 17, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 18, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 19, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 20, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 21, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 22, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 23, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 24, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 25, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 26, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 27, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 28, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 29, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 30, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 31, false),
				new InventoryItemModel(LootInventoryId, LootItemId, 3, 32, false));

			HasBeenAttacked = true;
			TeamController.Broadcast(player.TeamId, "Ihr habt den Tresor geöffnet!", NotificationType.SUCCESS);
			TeamController.Broadcast(new List<int>() { 1, 2 }, "Es wurde ein Alarm im Tresor der Staatsbank gemeldet!", NotificationType.WARN);
		}
	}
}