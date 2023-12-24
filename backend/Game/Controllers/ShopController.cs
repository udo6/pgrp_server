using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Shop;
using Database.Services;

namespace Game.Controllers
{
    public static class ShopController
	{
		private static Dictionary<ShopType, uint> PedModels = new()
		{
			{ ShopType.TWENTYFOURSEVEN, 416176080 },
			{ ShopType.AMMUNATION, 2651349821 },
			{ ShopType.TEAM, 1581098148 },
			{ ShopType.SWAT, 2374966032 },
			{ ShopType.MECHANIC, 3446096293 },
			{ ShopType.ALL_TEAMS_ONLY, 416176080 },
			{ ShopType.NO_BLIP, 416176080 },
			{ ShopType.FOOD, 416176080 },
		};

		public static void LoadShop(ShopModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 1.5f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.SHOP;
			shape.Size = 2f;

			var pedPos = PositionService.Get(model.PedPositionId);
			if (pedPos != null)
			{
				var ped = Alt.CreatePed(PedModels[model.Type], pedPos.Position, pedPos.Rotation);
				ped.Frozen = true;
				ped.Health = 8000;
				ped.Armour = 8000;
			}

			if (model.Type == ShopType.TWENTYFOURSEVEN)
			{
				var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
				blip.Name = "24/7 Shop";
				blip.Sprite = 52;
				blip.Color = 2;
				blip.ShortRange = true;
			}

			if (model.Type == ShopType.AMMUNATION)
			{
				var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
				blip.Name = "Ammunation";
				blip.Sprite = 110;
				blip.Color = 1;
				blip.ShortRange = true;
			}

			if (model.Type == ShopType.MECHANIC)
			{
				var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
				blip.Name = "Mechaniker";
				blip.Sprite = 402;
				blip.Color = 4;
				blip.ShortRange = true;
			}

			if (model.Type == ShopType.FOOD)
			{
				var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
				blip.Name = "Restaurant";
				blip.Sprite = 770;
				blip.Color = 4;
				blip.ShortRange = true;
			}
		}

		public static void ResetItemPrices()
		{
			var items = ShopService.GetAllItems();
			var random = new Random();
			foreach(var item in items)
				item.Price = random.Next(item.MinPrice, item.MaxPrice);

			ShopService.UpdateItems(items);
		}
	}
}