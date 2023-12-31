using AltV.Net;
using Core.Entities;
using Database.Services;
using Core.Enums;
using Core.Extensions;
using Database.Models.ClothesShop;
using AltV.Net.Elements.Entities;
using Core;

namespace Game.Controllers
{
    public static class ClothesShopController
	{
		public static void LoadClothesShop(ClothesShopModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 3f, 2f);
			shape.ShapeId = model.Id;
			shape.ShapeType = ColshapeType.CLOTHES_SHOP;
			shape.Size = 3f;

			if (model.Type == ClothesShopType.HATS) return;

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.ShortRange = true;

			switch (model.Type)
			{
				case ClothesShopType.CLOTHES:
					blip.Name = Config.DevMode ? $"Kleidungsladen #{model.Id}" : "Kleidungsladen";
					blip.Sprite = 73;
					blip.Color = 4;
					break;
				case ClothesShopType.MASKS:
					blip.Name = Config.DevMode ? $"Maskenladen #{model.Id}" : "Maskenladen";
					blip.Sprite = 362;
					blip.Color = 4;
					break;
				case ClothesShopType.JEWLERY:
					blip.Name = Config.DevMode ? $"Juwelier #{model.Id}" : "Juwelier";
					blip.Sprite = 617;
					blip.Color = 26;
					break;
			}
		}
	}
}