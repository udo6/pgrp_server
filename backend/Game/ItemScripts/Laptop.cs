using Core.Entities;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts
{
    public class Laptop : ItemScript
	{
		public Laptop() : base(13, false)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.Laptop) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			InventoryController.RemoveItem(inventory, slot, 1);

			player.Laptop = true;

			account.Laptop = true;
			AccountService.Update(account);
		}
	}
}