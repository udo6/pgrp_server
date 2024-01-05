using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.ItemScripts
{
	public class VehicleNote : ItemScript
	{
		public VehicleNote() : base(321, false)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (!player.IsInVehicle || player.Seat != 1) return;

			var veh = (RPVehicle)player.Vehicle;
			if (!VehicleController.IsVehicleOwner(veh, player)) return;

			player.ShowComponent("Inventory", false);
            player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Fahrzeug Notiz",
				Message = "Beschrifte die Notiz für das Fahrzeug.",
				Type = (int)InputType.TEXT,
				CallbackEvent = "Server:Vehicle:SetNote",
				CallbackArgs = new List<object>()
			}));
		}
	}
}
