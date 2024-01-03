using Core.Attribute;
using Core.Entities;
using Game.Controllers;

namespace Game.Commands.Player
{
	public static class DPOSCommands
	{
		[Command("impound")]
		public static void ImpoundVehicle(RPPlayer player, int vehId)
		{
			if (!player.LoggedIn || player.TeamId != 4 || !player.TeamDuty || vehId < 1) return;

			var shape = RPShape.All.FirstOrDefault(x => x.ShapeType == Core.Enums.ColshapeType.IMPOUND && x.Position.Distance(player.Position) <= x.Size);
			if (shape == null) return;

			var veh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (veh == null || veh.Position.Distance(shape.Position) > 20f) return;

			VehicleController.StoreVehicle(veh, 0);
			player.Notify("Ínformation", "Du hast ein Fahrzeug abgeschleppt!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}