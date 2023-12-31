using AltV.Net;
using AltV.Net.Elements.Entities;
using Core;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Barber;
using Database.Services;

namespace Game.Controllers
{
	public static class BarberController
	{
		public static void LoadBarber(BarberModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 3f, 2f);
			shape.ShapeId = model.Id;
			shape.ShapeType = ColshapeType.BARBER;
			shape.Size = 3f;

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.Name = Config.DevMode ? $"Friseur #{model.Id}" : "Friseur";
			blip.Sprite = 71;
			blip.Color = 4;
			blip.ShortRange = true;
		}
	}
}