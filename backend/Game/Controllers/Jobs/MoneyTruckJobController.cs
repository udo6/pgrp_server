using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using AltV.Net;
using Core.Entities;
using Database.Services;
using Core.Enums;
using Database.Models.MoneyTruckJob;
using Core.Extensions;

namespace Game.Controllers.Jobs
{
    public class MoneyTruckJobController
    {
        public static void LoadMoneyTruckJobs(MoneyTruckJobModel model)
        {
            var position = PositionService.Get(model.StartLocationId);
            if (position == null) return;

            var startJobShape = (RPShape)Alt.CreateColShapeCylinder(position.Position.Down(), 2f, 2f);
            startJobShape.ShapeId = model.Id;
            startJobShape.ShapeType = ColshapeType.MONEY_TRUCK_JOB_START;
            startJobShape.Size = 2f;

            var ped = Alt.CreatePed(PedModel.Casey, position.Position, position.Rotation);
            ped.Frozen = true;
            ped.Health = 8000;
            ped.Armour = 8000;

            var blip = Alt.CreateBlip(true, BlipType.Destination, position.Position, Array.Empty<IPlayer>());
            blip.Name = "Geldtransporter";
            blip.Sprite = (ushort)67;
            blip.Color = 46;
            blip.ShortRange = true;
        }

		public static void LoadMoneyTruckJobRoutePostion(MoneyTruckJobRoutePositionModel model)
		{
			var position = PositionService.Get(model.PositionId);
			if (position == null) return;

			var pickupPoint = (RPShape)Alt.CreateColShapeCylinder(position.Position.Down(), 1f, 2.5f);
			pickupPoint.ShapeId = model.Id;
			pickupPoint.ShapeType = ColshapeType.MONEY_TRUCK_JOB_PICKUP;
			pickupPoint.Size = 1f;
		}
	}
}
