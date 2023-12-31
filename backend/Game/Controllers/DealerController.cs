using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Dealer;
using Database.Services;

namespace Game.Controllers
{
	public static class DealerController
	{
		private static Random Random = new Random();

		public static void LoadDealer(DealerModel model, bool forceActivate = false)
		{
			if (!forceActivate && Random.Next(0, 101) > 10) return;

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
		}

		public static void ResetItemPrices()
		{
			var items = DealerService.GetAllItems();
			var random = new Random();
			foreach (var item in items)
				item.Price = random.Next(item.MinPrice, item.MaxPrice);

			DealerService.UpdateItems(items);
		}

		public static void ResetDealers(List<DealerModel> models)
		{
			foreach(var model in models)
				model.Active = false;

			DealerService.Update(models);
			ResetItemPrices();
		}
	}
}
