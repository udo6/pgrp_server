using Core.Entities;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts
{
    public class HouseLock : ItemScript
	{
		public HouseLock() : base(31, false)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if(player.Dimension < 1)
			{
				player.Notify("Information", "Du musst dich in deinem Haus befinden!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var house = HouseService.GetByOwner(player.DbId);
			if (house == null || player.Dimension != house.Id)
			{
				player.Notify("Information", "Du musst dich in deinem Haus befinden!", Core.Enums.NotificationType.ERROR);
				return;
			}

			InventoryController.RemoveItem(inventory, slot, 1);

			house.KeyHolderId = 0;
			HouseService.Update(house);
			player.Notify("Information", "Du hast die Schlösser ausgetauscht!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}