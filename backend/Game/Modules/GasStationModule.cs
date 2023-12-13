using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class GasStationModule
	{
		[Initialize]
		public static void Initialize()
		{
			var stations = GasStationService.GetAll();
			foreach (var model in stations)
				GasStationController.LoadGasStation(model);

			GasStationController.ResetPrices(stations);

			Alt.OnClient<RPPlayer, int, int, int>("Server:GasStation:StartFueling", FuelVehicle);
		}

		private static void FuelVehicle(RPPlayer player, int vehicleId, int stationId, int value)
		{
			if (player.IsInVehicle) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var vehicle = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId);
			if (vehicle == null) return;

			var station = GasStationService.Get(stationId);
			if (station == null) return;

			var price = 20 * value;

			if(account.Money < price)
			{
				player.Notify("Information", "Du hast nicht genug Geld dabei!", Core.Enums.NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(Core.Enums.AnimationType.DRILL);
			player.StartInteraction(() =>
			{
				PlayerController.RemoveMoney(player, price);
				VehicleController.SetVehicleFuel(vehicle, vehicle.Fuel + value);
				player.Notify("Information", $"Du hast {value} Liter in dein Fahrzeug getankt!", Core.Enums.NotificationType.SUCCESS);
			}, 300 * value);
		}
	}
}