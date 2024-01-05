using Core.Entities;
using Database.Models.Inventory;
using Game.Controllers;

namespace Game.ItemScripts
{
	public class GasCan : ItemScript
	{
		public GasCan() : base(378, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (!player.IsInVehicle) return;

			var vehicle = (RPVehicle)player.Vehicle;

			vehicle.SetFuel(Math.Clamp(vehicle.Fuel + 15, 0, vehicle.MaxFuel));
			InventoryController.RemoveItem(inventory, slot, 1);
			player.Notify("Information", "Du hast dein Fahrzeug befüllt!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
