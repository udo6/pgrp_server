using Core.Entities;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts
{
    public class Backpack : ItemScript
	{
		public Backpack() : base(14, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null || account.Backpack) return;

			InventoryController.RemoveItem(inventory, slot, 1);

			account.Backpack = true;
			AccountService.Update(account);

			inventory.Slots = 16;
			inventory.MaxWeight = 60f;
			InventoryService.Update(inventory);
		}
	}
}