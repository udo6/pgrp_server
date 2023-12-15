using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.GasStation;
using Database.Services;

namespace Game.Controllers
{
    public static class GasStationController
	{
		public static void LoadGasStation(GasStationModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 20f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.GAS_STATION;
			shape.Size = 20f;

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.Name = "Tankstelle";
			blip.Sprite = 361;
			blip.Color = 51;
			blip.ShortRange = true;
		}

		public static void ResetPrices(List<GasStationModel> models)
		{
			var random = new Random();

			foreach(var model in models)
				model.Price = random.Next(model.MinPrice, model.MaxPrice);

			GasStationService.Update(models);
		}
	}
}