using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Inventory;
using Database.Models.Vehicle;
using Database.Services;

namespace Game.Commands.Gamedesign
{
	public static class VehicleCommands
	{
		[Command("createvehicle")]
		public static void CreateVehicle(RPPlayer player, string plate, int garageId, int baseId, int owner, int type)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var baseModel = VehicleService.GetBase(baseId);
			if(baseModel == null) return;

			var pos = new PositionModel();
			PositionService.Add(pos);

			var trunk = new InventoryModel(baseModel.TrunkSlots, baseModel.TrunkWeight, InventoryType.TRUNK);
			var glovebox = new InventoryModel(baseModel.GloveBoxSlots, baseModel.GloveBoxWeight, InventoryType.GLOVEBOX);
			InventoryService.Add(trunk, glovebox);

			var tune = new TuningModel();
			TuningService.Add(tune);

			var veh = new VehicleModel(owner, 0, true, plate.Replace('_', ' '), "", 100, (OwnerType)type, garageId, pos.Id, trunk.Id, glovebox.Id, baseId, tune.Id);
			VehicleService.AddVehicle(veh);

			player.Notify("Administration", "Du hast ein Fahrzeug vergeben!", NotificationType.SUCCESS);
		}

		[Command("setmod")]
		public static void SetMod(RPPlayer player, int cat, int val)
		{
			if ((!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) || !player.IsInVehicle) return;

			player.Vehicle.SetMod((byte)cat, (byte)val);
		}
	}
}