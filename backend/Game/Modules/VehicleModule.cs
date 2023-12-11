using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class VehicleModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, string>("Server:Vehicle:SetNote", SetNote);
			Alt.OnClient<RPPlayer, int, int>("Server:Vehicle:GiveKey", GivePlayerKey);
			Alt.OnClient<RPPlayer, int>("Server:Vehicle:Eject", EjectPlayer);
			Alt.OnClient<RPPlayer, float>("Server:Vehicle:UpdateFuel", UpdateFuel);
			Alt.OnClient<RPPlayer>("Server:Vehicle:Lock", Lock);
			Alt.OnClient<RPPlayer>("Server:Vehicle:LockTrunk", LockTrunk);
			Alt.OnClient<RPPlayer>("Server:Vehicle:ToggleSirenState", ToggleSirenState);

			foreach (var vehicle in VehicleService.GetParked())
				VehicleController.LoadVehicle(vehicle);
		}

		private static void SetNote(RPPlayer player, string note)
		{
			if (!player.IsInVehicle || player.Seat != 0) return;

			var veh = (RPVehicle)player.Vehicle;
			if (!VehicleController.IsVehicleOwner(veh, player)) return;

			var model = VehicleService.Get(veh.DbId);
			if (model == null) return;

			model.Note = note;
			VehicleService.UpdateVehicle(model);
			player.Notify("Information", "Du hast eine Notiz an das Fahrzeug angebracht!", NotificationType.SUCCESS);
		}

		private static void GivePlayerKey(RPPlayer player, int targetId, int vehicleId)
		{
			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			var vehicle = VehicleService.Get(vehicleId);
			if (vehicle == null) return;

			var localVeh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehicleId);
			if (localVeh != null)
			{
				localVeh.KeyHolderId = targetId;
			}

			player.Notify("Information", $"Du hast {target.Name} einen Fahrzeug Schlüssel gegeben!", NotificationType.INFO);
			target.Notify("Information", $"{player.Name} hat dir einen Fahrzeug Schlüssel gegeben!", NotificationType.INFO);

			vehicle.KeyHolderId = target.DbId;
			VehicleService.UpdateVehicle(vehicle);
		}

		private static void EjectPlayer(RPPlayer player, int seat)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.Seat != 1) return;

			if (seat == 1)
			{
				player.Notify("Fahrzeug", "Du kannst dich nicht selbst rauswerfen!", NotificationType.ERROR);
				return;
			}

			var target = RPPlayer.All.FirstOrDefault(x => x.Vehicle == player.Vehicle && x.Seat == seat);
			if (target == null) return;

			target.Emit("Client:Vehicle:Exit");
			target.Notify("Fahrzeug", "Du wurdest aus dem Fahrzeug geworfen!", NotificationType.INFO);
		}

		private static void UpdateFuel(RPPlayer player, float mod)
		{
			if (!player.LoggedIn || !player.IsInVehicle || float.IsNaN(mod)) return;

			var veh = (RPVehicle)player.Vehicle;
			VehicleController.SetVehicleFuel(veh, veh.Fuel - mod);
		}

		private static void Lock(RPPlayer player)
		{
			if (!player.LoggedIn) return;

			var vehicle = player.IsInVehicle ? (RPVehicle)player.Vehicle : VehicleController.GetClosestVehicle(player.Position, 5f);
			if (vehicle == null || !VehicleController.IsVehicleOwner(vehicle, player)) return;

			vehicle.SetLockState(!vehicle.Locked);
			if (vehicle.Locked) player.Notify("FAHRZEUG", $"Fahrzeug abgeschlossen.", NotificationType.ERROR);
			else player.Notify("FAHRZEUG", $"Fahrzeug aufgeschlossen.", NotificationType.SUCCESS);
		}

		private static void LockTrunk(RPPlayer player)
		{
			if (!player.LoggedIn) return;

			var vehicle = player.IsInVehicle ? (RPVehicle)player.Vehicle : VehicleController.GetClosestVehicle(player.Position, 5f);
			if (vehicle == null || vehicle.Locked) return;

			vehicle.SetTrunkState(!vehicle.TrunkLocked);
			if (vehicle.TrunkLocked) player.Notify("Fahrzeug", $"Kofferraum abgeschlossen.", NotificationType.ERROR);
			else player.Notify("Fahrzeug", $"Kofferraum aufgeschlossen.", NotificationType.SUCCESS);
		}

		private static void ToggleSirenState(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInVehicle) return;

			var veh = (RPVehicle)player.Vehicle;
			veh.SetSirenSoundState(!veh.SirenSoundActive);
		}
	}
}