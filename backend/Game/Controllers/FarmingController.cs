using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Farming;
using Database.Services;

namespace Game.Controllers
{
    public static class FarmingController
	{
		public static List<FarmingSpotModel> CachedFarmingSpots = new();

		public static void LoadFarming(FarmingModel model)
		{
			foreach (var spot in FarmingService.GetFromFarming(model.Id))
				LoadFarmingSpot(model, spot);
		}

		public static void LoadFarmingSpot(FarmingModel model, FarmingSpotModel spot)
		{
			var pos = PositionService.Get(spot.PositionId);
			if (pos == null) return;

			spot.Shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
			spot.Shape.Id = spot.Id;
			spot.Shape.ShapeType = ColshapeType.FARMING_SPOT;
			spot.Shape.Size = 2f;

			spot.Object = Alt.CreateObject(model.ObjectHash, pos.Position.Down(), pos.Rotation);
			spot.Object.Frozen = true;

			CachedFarmingSpots.Add(spot);
		}

		public static void EnableSpot(FarmingSpotModel spot)
		{
			if (spot.Shape == null) return;

			var model = FarmingService.Get(spot.FarmingId);
			if (model == null) return;

			spot.Object.Streamed = true;
			spot.Health = 100;
		}

		public static void DisableSpot(FarmingSpotModel spot)
		{
			spot.Object.Streamed = false;
			spot.Despawned = DateTime.Now;
			spot.Health = 0;
		}

		public static void StopFarming(RPPlayer player)
		{
			player.IsFarming = false;
			player.StopAnimation();
			player.FarmingShape = null;
			player.FarmingSpotId = 0;
			player.Emit("Client:PlayerModule:SetFarming", false);
		}
	}
}