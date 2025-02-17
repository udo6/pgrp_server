﻿using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Core.Models.Dealer;
using Database.Models.Dealer;
using Database.Services;

namespace Game.Controllers
{
	public static class DealerController
	{
		public static List<DealerCache> DealerCache = new();

		public static void LoadDealer(DealerModel model, List<DealerItemModel> items)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
			shape.ShapeId = model.Id;
			shape.ShapeType = ColshapeType.DEALER;
			shape.Size = 2f;

			var ped = Alt.CreatePed(0xE497BBEF, pos.Position, pos.Rotation);
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;

			model.Active = true;
			DealerService.Update(model);

			var dealerCache = new DealerCache(model.Id);
			foreach(var item in items)
			{
				dealerCache.Items.Add(new(item.Id, new Random().Next(item.MinPrice, item.MaxPrice), item.SellCap));
            }

            DealerCache.Add(dealerCache);
        }

		public static void ResetDealers(List<DealerModel> models)
		{
			foreach(var model in models)
				model.Active = false;

			DealerService.Update(models);
		}
	}
}
