using AltV.Net;
using Core.Entities;
using Database.Services;
using Core.Enums;
using Core.Extensions;
using Database.Models.Garage;
using AltV.Net.Elements.Entities;

namespace Game.Controllers
{
    public static class GarageController
	{
		public static void LoadGarage(GarageModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
			shape.ShapeId = model.Id;
			shape.ShapeType = ColshapeType.GARAGE;
			shape.Size = 2f;

			var pedPos = PositionService.Get(model.PedPositionId);
			if (pedPos != null)
			{
				var ped = Alt.CreatePed(3446096293, pedPos.Position, pedPos.Rotation);
				ped.Frozen = true;
				ped.Health = 8000;
				ped.Armour = 8000;
			}

			if (model.Owner < 1)
			{
				var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
				blip.Name = "Garage";
				blip.Sprite = 856;
				blip.Color = 4;
				blip.ShortRange = true;
			}
		}
	}
}