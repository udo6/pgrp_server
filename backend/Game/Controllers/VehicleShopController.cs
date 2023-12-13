using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Vehicle;
using Database.Models.VehicleShop;
using Database.Services;

namespace Game.Controllers
{
    public static class VehicleShopController
	{
		public static void LoadVehicleShop(VehicleShopModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.VEHICLE_SHOP;
			shape.Size = 2f;

			var pedPos = PositionService.Get(model.PedPositionId);
			if (pedPos != null)
			{
				var ped = Alt.CreatePed(0x3AE4A33B, pedPos.Position, pedPos.Rotation);
				ped.Frozen = true;
				ped.Health = 8000;
				ped.Armour = 8000;
			}

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.Name = "Fahrzeughändler";
			blip.Sprite = 225;
			blip.Color = 4;
			blip.ShortRange = true;

			var bases = VehicleService.GetAllBases();
			foreach (var item in VehicleShopService.GetItemsFromShop(model.Id))
				LoadVehicleShopItem(item, bases);
		}

		public static void LoadVehicleShopItem(VehicleShopItemModel model, List<VehicleBaseModel> bases)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var baseModel = bases.FirstOrDefault(x => x.Id == model.VehicleBaseId);
			if (baseModel == null) return;

			var veh = (RPVehicle)Alt.CreateVehicle(baseModel.Hash, pos.Position, pos.Rotation);
			veh.PrimaryColorRgb = new(0, 0, 0, 255);
			veh.SecondaryColorRgb = new(0, 0, 0, 255);
			veh.PearlColor = 0;
			veh.NumberplateText = "";
			veh.Frozen = true;
			veh.SetLockState(true);
			veh.SetEngineState(false);
			veh.SetFuel(0);
			veh.SetMaxFuel(0);
		}
	}
}