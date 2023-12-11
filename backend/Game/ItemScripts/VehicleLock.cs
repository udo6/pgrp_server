using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts
{
    public class VehicleLock : ItemScript
	{
		public VehicleLock() : base(27, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (!player.IsInVehicle) return;

			var veh = (RPVehicle)player.Vehicle;
			var vehicle = VehicleService.Get(veh.DbId);
			if (vehicle == null || vehicle.Type != OwnerType.PLAYER || vehicle.OwnerId != player.DbId) return;

			player.Notify("Information", "Du hast das Schloss ausgetauscht!", NotificationType.SUCCESS);

			vehicle.KeyHolderId = 0;
			VehicleService.UpdateVehicle(vehicle);
			InventoryController.RemoveItem(inventory, slot, 1);
		}
	}
}