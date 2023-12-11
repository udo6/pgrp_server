using Core.Entities;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts
{
    public class Phone : ItemScript
	{
		public Phone() : base(12, false)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.Phone) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			InventoryController.RemoveItem(inventory, slot, 1);

			player.Phone = true;

			account.Phone = true;
			AccountService.Update(account);
		}
	}
}