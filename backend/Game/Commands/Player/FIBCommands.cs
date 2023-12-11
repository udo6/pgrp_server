using Core.Attribute;
using Core.Entities;
using Core.Models.NativeMenu;
using Database.Models.Account;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;
using System.ComponentModel;

namespace Game.Commands.Player
{
    public static class FIBCommands
	{
		[Command("wanteds")]
		public static void ShowWanteds(RPPlayer player)
		{
			if (player.TeamId != 2 || !player.TeamDuty) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || account.TeamRank < 3) return;

			var nativeItems = new List<NativeMenuItem>();
			foreach(var target in RPPlayer.All.ToList())
			{
				if (!target.LoggedIn || !CrimeService.HasPlayerCrimes(target.DbId)) continue;

				nativeItems.Add(new(target.Name, false, "Server:FIB:LocatePlayer", target.DbId));
			}

			player.ShowNativeMenu(true, new("Wanteds", nativeItems));
		}

		[Command("findhouse")]
		public static void FindHouse(RPPlayer player, int houseId)
		{
			if (player.TeamId != 2 || !player.TeamDuty) return;

			var house = HouseService.Get(houseId);
			if (house == null) return;

			var pos = PositionService.Get(house.PositionId);
			if (pos == null) return;

			player.Emit("Client:PlayerModule:SetWaypoint", pos.Position.X, pos.Position.Y);
			player.Notify("Information", $"Du hast Haus {houseId} geortet!", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("friskhouse")]
		public static void FriskHouse(RPPlayer player)
		{
			if (player.TeamId != 2 || !player.TeamDuty || player.Dimension == 0) return;

			var house = HouseService.Get(player.Dimension);
			if (house == null || house.OwnerId <= 0) return;

			var pos = PositionService.Get(HouseController.HousePositions[house.Type]);
			if (pos == null || player.Position.Distance(pos.Position) > 10f) return;

			var inventory = InventoryService.Get(player.InventoryId);
			var container = InventoryService.Get(house.InventoryId);
			if (inventory == null || container == null) return;

			var itemBases = InventoryService.GetItems();
			var inventoryItems = InventoryService.GetInventoryItems2(inventory.Id, container?.Id);
			var invItems = InventoryController.GetInventoryItems(inventoryItems.Items1, itemBases);
			var ctnItems = InventoryController.GetInventoryItems(inventoryItems.Items2, itemBases);

			player.ShowComponent("Inventory", true, JsonConvert.SerializeObject(new
			{
				Inventory = inventory,
				InventoryItems = invItems,
				ContainerLabel = $"Haus {house.Id}",
				Container = container,
				ContainerItems = ctnItems,
				Loadout = Array.Empty<LoadoutModel>(),
				GiveItemTarget = -1,
				SearchTargetId = 0
			}));
		}
	}
}