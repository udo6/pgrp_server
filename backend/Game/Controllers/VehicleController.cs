using AltV.Net;
using AltV.Net.Data;
using Core.Entities;
using Database.Services;
using Core.Enums;
using Database.Models.Vehicle;
using Database.Models;
using AltV.Net.Elements.Entities;

namespace Game.Controllers
{
    public static class VehicleController
	{
		public static void LoadVehicle(VehicleModel model)
		{
			var baseModel = VehicleService.GetBase(model.BaseId);
			if (baseModel == null) return;

			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var veh = (RPVehicle)Alt.CreateVehicle(baseModel.Hash, pos.Position, pos.Rotation);
			veh.DbId = model.Id;
			veh.OwnerId = model.OwnerId;
			veh.KeyHolderId = model.KeyHolderId;
			veh.TuningId = model.TuningId;
			veh.TrunkId = model.TrunkId;
			veh.GloveBoxId = model.GloveBoxId;
			veh.BaseId = model.BaseId;
			veh.PositionId = model.PositionId;
			veh.OwnerType = model.Type;
			veh.GarageType = baseModel.GarageType;
			veh.SetFuel(model.Fuel);
			veh.SetMaxFuel(baseModel.MaxFuel);

			veh.NumberplateText = model.Plate;

			ApplyVehicleTuning(veh);
		}

		public static void LoadVehicle(VehicleModel model, PositionModel position)
		{
			var baseModel = VehicleService.GetBase(model.BaseId);
			if (baseModel == null) return;

			var veh = (RPVehicle)Alt.CreateVehicle(baseModel.Hash, position.Position, position.Rotation);
			veh.DbId = model.Id;
			veh.OwnerId = model.OwnerId;
			veh.KeyHolderId = model.KeyHolderId;
			veh.TuningId = model.TuningId;
			veh.TrunkId = model.TrunkId;
			veh.GloveBoxId = model.GloveBoxId;
			veh.BaseId = model.BaseId;
			veh.PositionId = model.PositionId;
			veh.OwnerType = model.Type;
			veh.GarageType = baseModel.GarageType;
			veh.SetFuel(model.Fuel);
			veh.SetMaxFuel(baseModel.MaxFuel);

			veh.NumberplateText = model.Plate;

			ApplyVehicleTuning(veh);
		}

		public static void StoreVehicle(RPVehicle vehicle, int garageId, bool ignoreGarage = false)
		{
			if(vehicle.DbId > 0)
			{
				var model = VehicleService.Get(vehicle.DbId);
				if (model == null) return;

				if(!ignoreGarage) model.GarageId = garageId;
				model.Parked = true;
				VehicleService.UpdateVehicle(model);

				var pos = PositionService.Get(vehicle.PositionId);
				if (pos == null) return;

				pos.Position = vehicle.Position;
				pos.Rotation = vehicle.Rotation;

				PositionService.Update(pos);
			}

			RPVehicle.All.Remove(vehicle);
			vehicle.Destroy();
		}

		public static RPVehicle? GetClosestVehicle(Position position, float range)
		{
			var vehicles = RPVehicle.All.Where(x => x.Position.Distance(position) <= range).ToList();
			RPVehicle? vehicle = null;
			var dist = range;

			for (var i = 0; i < vehicles.Count; i++)
			{
				var distance = position.Distance(vehicles[i].Position);
				if (distance < dist)
				{
					dist = distance;
					vehicle = vehicles[i];
				}
			}

			return vehicle;
		}

		public static bool IsVehicleOwner(RPVehicle vehicle, RPPlayer player)
		{
			switch (vehicle.OwnerType)
			{
				case OwnerType.PLAYER:
					return vehicle.OwnerId == player.DbId || vehicle.KeyHolderId == player.DbId;
				case OwnerType.TEAM:
					return vehicle.OwnerId == player.TeamId;
				case OwnerType.SWAT:
					return player.SWATDuty;
			}

			return false;
		}

		public static void SetVehicleFuel(RPVehicle vehicle, float fuel)
		{
			var veh = VehicleService.Get(vehicle.DbId);
			if (veh == null) return;

			veh.Fuel = fuel;
			VehicleService.UpdateVehicle(veh);

			vehicle.Fuel = fuel;
			vehicle.SetStreamSyncedMetaData("FUEL", fuel);
			if (fuel == 0) vehicle.SetEngineState(false);
		}

		public static void ApplyVehicleTuning(RPVehicle veh)
		{
			var tuning = TuningService.Get(veh.TuningId);
			if (tuning == null) return;

			veh.PrimaryColor = tuning.PrimaryColor;
			veh.SecondaryColor = tuning.SecondaryColor;
			veh.PearlColor = tuning.PearlColor;
			veh.NeonColor = new(tuning.NeonR, tuning.NeonG, tuning.NeonB, 255);
			veh.WindowTint = tuning.WindowTint;
			veh.HeadlightColor = tuning.HeadlightColor;

			veh.SetWheels(tuning.WheelType, tuning.Wheels);
			veh.WheelColor = tuning.WheelColor;

			veh.NumberplateIndex = tuning.PlateHolders;

			veh.SetMod(0, tuning.Spoiler);
			veh.SetMod(1, tuning.FrontBumper);
			veh.SetMod(2, tuning.RearBumper);
			veh.SetMod(3, tuning.SideSkirt);
			veh.SetMod(4, tuning.Exhaust);
			veh.SetMod(5, tuning.Frame);
			veh.SetMod(6, tuning.Grille);
			veh.SetMod(7, tuning.Hood);
			veh.SetMod(8, tuning.Fender);
			veh.SetMod(9, tuning.RightFender);
			veh.SetMod(10, tuning.Roof);
			veh.SetMod(11, tuning.Engine);
			veh.SetMod(12, tuning.Brakes);
			veh.SetMod(13, tuning.Transmission);
			veh.SetMod(14, tuning.Horns);
			veh.SetMod(15, tuning.Suspension);
			veh.SetMod(16, tuning.Armor);
			veh.SetMod(18, tuning.Turbo);
			veh.SetMod(22, tuning.Xenon);
			veh.SetMod(25, tuning.PlateHolders);
			veh.SetMod(27, tuning.TrimDesign);
			veh.SetMod(48, tuning.Livery);

			veh.SetNeonActive(tuning.Neons, tuning.Neons, tuning.Neons, tuning.Neons);
		}
	}
}