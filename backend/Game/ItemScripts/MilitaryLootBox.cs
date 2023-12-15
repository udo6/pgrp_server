using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Game.Controllers;
using Game.Modules.Scenario;

namespace Game.ItemScripts
{
	public class MilitaryLootBox : ItemScript
	{
		public MilitaryLootBox() : base(363, true, ItemType.DEFAULT)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			InventoryController.RemoveItem(inventory, slot, 1);

			var loot = LootdropModule.GetLoot();
			if (loot == null) return;

			var itemAmount = new Random().Next(1, loot.StackSize + 1);
			InventoryController.AddItem(inventory.Id, loot.Id, itemAmount);
			player.Notify("Information", $"Du hast {itemAmount}x {loot.Name} in der Kiste gefunden!", NotificationType.SUCCESS);
		}
	}
}
