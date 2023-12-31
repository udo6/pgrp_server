using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;

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
	}
}