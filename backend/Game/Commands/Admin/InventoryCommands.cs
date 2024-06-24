using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Game.Modules;
using Newtonsoft.Json;

namespace Game.Commands.Admin
{
	public static class InventoryCommands
	{
		[Command("giveitem")]
		public static void GiveItem(RPPlayer player, int itemId, int amount)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = InventoryService.GetItem(itemId);
			if (item == null) return;

			InventoryController.AddItem(inventory, item, amount);
		}

		[Command("clearinventory")]
		public static void ClearInventory(RPPlayer player, string targetName)
		{
			if (player.AdminRank < Core.Enums.AdminRank.ADMINISTRATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			InventoryService.ClearInventoryItems(target.InventoryId);
			player.Notify("Administration", $"Du hast das Inventar von {target.Name} gecleared!", Core.Enums.NotificationType.SUCCESS);
			target.Notify("Administration", $"Dein Inventar wurde von {player.Name} gecleared!", Core.Enums.NotificationType.WARN);
		}

		[Command("openinventory")]
		public static void OpenInventory(RPPlayer player, int inventoryId)
		{
			if (player.AdminRank < Core.Enums.AdminRank.ADMINISTRATOR) return;

			var itemBases = InventoryService.GetItems();
			var inventory = InventoryService.Get(player.InventoryId);
			var container = InventoryService.Get(inventoryId);
			if (inventory == null || container == null) return;

			var inventoryItems = InventoryService.GetInventoryItems2(inventory.Id, container.Id);
			var invItems = InventoryController.GetInventoryItems(inventoryItems.Items1, itemBases);
			var ctnItems = InventoryController.GetInventoryItems(inventoryItems.Items2, itemBases);

			player.ShowComponent("Inventory", true, JsonConvert.SerializeObject(new
			{
				Inventory = inventory,
				InventoryItems = invItems,
				ContainerLabel = InventoryModule.GetContainerLabel(container.Type),
				Container = container,
				ContainerItems = ctnItems,
				Loadout = Array.Empty<object>(),
				GiveItemTarget = -1,
				SearchTargetId = -1
			}));
		}
	}
}