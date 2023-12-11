using Core.Entities;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts
{
    public class WarehouseLock : ItemScript
	{
		public WarehouseLock() : base(32, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if(player.Dimension < 1)
			{
				player.Notify("Information", "Du musst dich in deiner Lagerhalle befinden!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var warehouse = WarehouseService.GetByOwner(player.DbId, Core.Enums.OwnerType.PLAYER);
			if (warehouse == null || player.Dimension != warehouse.Id)
			{
				player.Notify("Information", "Du musst dich in deiner Lagerhalle befinden!", Core.Enums.NotificationType.ERROR);
				return;
			}

			InventoryController.RemoveItem(inventory, slot, 1);

			warehouse.KeyHolderId = 0;
			WarehouseService.Update(warehouse);
			player.Notify("Information", "Du hast die Schlösser ausgetauscht!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}