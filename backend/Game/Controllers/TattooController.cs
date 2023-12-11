using AltV.Net.Elements.Entities;
using AltV.Net;
using Core.Entities;
using Database.Models.Tattoo;
using Database.Services;
using Core.Extensions;
using Core.Enums;

namespace Game.Controllers
{
	public static class TattooController
	{
		public static void LoadTattooShop(TattooShopModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.TATTOO_SHOP;
			shape.Size = 2f;

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.Name = "Tattoo Studio";
			blip.Sprite = 75;
			blip.Color = 4;
			blip.ShortRange = true;
		}
	}
}