using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Wardrobe;
using Database.Services;

namespace Game.Controllers
{
    public static class WardrobeController
	{
		public static void LoadWardrobe(WardrobeModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.WARDROBE;
			shape.Size = 2f;
			shape.Dimension = model.Dimension;
		}
	}
}