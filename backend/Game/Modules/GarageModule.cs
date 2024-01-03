using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Core.Enums;
using Newtonsoft.Json;
using Database.Models.Vehicle;
using Database.Models.Garage;

namespace Game.Modules
{
    public static class GarageModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var garage in GarageService.GetAll())
				GarageController.LoadGarage(garage);

			Alt.OnClient<RPPlayer, int>("Server:Garage:Open", Open);
			Alt.OnClient<RPPlayer, int, int>("Server:Garage:TakeVehicle", TakeVehicle);
			Alt.OnClient<RPPlayer, int, int>("Server:Garage:ParkVehicle", ParkVehicle);
		}

		private static void Open(RPPlayer player, int garageId)
		{
			var garage = GarageService.Get(garageId);
			if (garage == null || !CheckGaragePermission(player, garage)) return;

			var pos = PositionService.Get(garage.PositionId);
			if (pos == null || player.Position.Distance(pos.Position) > 2f) return;

			var parkedVehicles = VehicleService.GetParkedPlayerVehicles(GetOwnerId(player, garage.OwnerType), garage.Id, garage.OwnerType);
			var tookVehicles = RPVehicle.All.Where(x =>
			x.Position.Distance(pos.Position) < 55f &&
			VehicleController.IsVehicleOwner(x, player) &&
			x.GarageType == garage.Type &&
			x.OwnerType == garage.OwnerType).ToList();

			player.ShowComponent("Garage", true, JsonConvert.SerializeObject(new { Id = garage.Id, Vehicles = GetVehicleData(parkedVehicles, tookVehicles) }));
		}

		private static void TakeVehicle(RPPlayer player, int garageId, int vehId)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.GARAGE);
			if (shape == null) return;

			var vehicle = VehicleService.Get(vehId);
			if(vehicle == null || !vehicle.Parked) return;

			var spawns = GarageService.GetGarageSpawns(garageId);

			var spawn = spawns.FirstOrDefault(x => RPVehicle.All.FirstOrDefault(e => PositionService.Get(x.PositionId)!.Position.Distance(e.Position) < 2) == null);

			if(spawn == null)
			{
				player.Notify("Information", "Es ist kein Ausparkpunkt frei!", NotificationType.ERROR);
				return;
			}

			var pos = PositionService.Get(spawn.PositionId);
			if (pos == null) return;

			var baseModel = VehicleService.GetBase(vehicle.BaseId);
			if (baseModel == null) return;

			vehicle.Parked = false;
			VehicleService.UpdateVehicle(vehicle);

			VehicleController.LoadVehicle(vehicle, pos);

			var vehPos = PositionService.Get(vehicle.PositionId);
			if (vehPos == null) return;

			vehPos.Position = pos.Position;
			vehPos.Rotation = pos.Rotation;
			PositionService.Update(vehPos);
		}

		private static void ParkVehicle(RPPlayer player, int garageId, int vehId)
		{
			var veh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (veh == null || player.Position.Distance(veh.Position) > 30f) return;

			var garage = GarageService.Get(garageId);
			if (garage == null) return;

			var garagePos = PositionService.Get(garage.PositionId);
			if (garagePos == null || veh.Position.Distance(garagePos.Position) > 30f) return;

			var vehicle = VehicleService.Get(vehId);
			if (vehicle == null) return;

			vehicle.Parked = true;
			vehicle.GarageId = garageId;
			VehicleService.UpdateVehicle(vehicle);
			veh.Delete();

			var pos = PositionService.Get(vehicle.PositionId);
			if (pos == null) return;

			pos.Position = veh.Position;
			pos.Rotation = veh.Rotation;
			PositionService.Update(pos);
		}

		private static bool CheckGaragePermission(RPPlayer player, GarageModel garage)
		{
			return garage.OwnerType == OwnerType.PLAYER || (garage.OwnerType == OwnerType.TEAM && player.TeamId == garage.Owner) || (garage.OwnerType == OwnerType.SWAT && player.SWATDuty);
		}

		private static List<object> GetVehicleData(List<VehicleModel> parkedVehicles, List<RPVehicle> vehicles)
		{
			var data = new List<object>();
			var allVehicles = new List<VehicleModel>(parkedVehicles);
			foreach (var veh in vehicles)
			{
				var model = VehicleService.Get(veh.DbId);
				if (model == null) continue;

				allVehicles.Add(model);
			}

			foreach(var vehicle in allVehicles)
			{
				var baseModel = VehicleService.GetBase(vehicle.BaseId);
				if (baseModel == null) continue;

				data.Add(new
				{
					Id = vehicle.Id,
					Name = baseModel.Name,
					Plate = vehicle.Plate,
					Parked = vehicle.Parked,
					Note = vehicle.Note
				});
			}

			return data;
		}

		private static int GetOwnerId(RPPlayer player, OwnerType garageType)
		{
			switch (garageType)
			{
				case OwnerType.PLAYER: return player.DbId;
				case OwnerType.TEAM: return player.TeamId;
				default: return 0;
			}
		}
	}
}