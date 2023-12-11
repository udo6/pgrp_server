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
		public static void LoadDealer(DealerModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.DEALER;
			shape.Size = 2f;

			var ped = Alt.CreatePed(3446096293, pos.Position, pos.Rotation);
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;
		}
	}
}
