using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class FarmingModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in FarmingService.GetAll())
				FarmingController.LoadFarming(model);

			Alt.OnClient<RPPlayer>("Server:Farming:Start", StartFarming);
			Alt.OnClient<RPPlayer>("Server:Farming:Stop", FarmingController.StopFarming);
		}

		private static void StartFarming(RPPlayer player)
		{
			if (player.IsInVehicle) return;

			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.FARMING_SPOT);
			if (shape == null) return;

			var spot = FarmingController.CachedFarmingSpots.FirstOrDefault(x => x.Id == shape.ShapeId);
			if (spot == null) return;

			var model = FarmingService.Get(spot.FarmingId);
			if (model == null) return;

			if(model.NeededItem > 0 && InventoryService.HasItems(player.InventoryId, model.NeededItem) < 1)
			{
				var item = InventoryService.GetItem(model.NeededItem);
				if (item == null) return;

				player.Notify("Information", $"Du benögtigst 1 {item.Name}", NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(model.AnimationDict, model.AnimationName, 1);
			player.FarmingShape = shape;
			player.FarmingSpotId = spot.Id;
			player.IsFarming = true;
			player.Emit("Client:PlayerModule:SetFarming", true);
		}

		[EveryFiveSeconds]
		public static void PlayerTick()
		{
			var itemBases = InventoryService.GetItems();
			var models = FarmingService.GetAll();
			var random = new Random();

			foreach (var player in RPPlayer.All.ToList())
			{
				if (!player.IsFarming) continue;

				if (player.FarmingShape == null || player.Position.Distance(player.FarmingShape.Position) > player.FarmingShape.Size)
				{
					FarmingController.StopFarming(player);
					continue;
				}

				var spot = FarmingController.CachedFarmingSpots.FirstOrDefault(x => x.Id == player.FarmingSpotId);
				if (spot == null || spot.Health <= 0)
				{
					FarmingController.StopFarming(player);
					continue;
				}

				var model = models.FirstOrDefault(x => x.Id == spot.FarmingId);
				if (model == null)
				{
					FarmingController.StopFarming(player);
					continue;
				}

				var item = itemBases.FirstOrDefault(x => x.Id == model.GainItem);
				if (item == null)
				{
					FarmingController.StopFarming(player);
					continue;
				}

				// spot.Health--;
				if (spot.Health <= 0) FarmingController.DisableSpot(spot);

				var amount = random.Next(model.MinGain, model.MaxGain);
				if(!InventoryController.AddItem(player.InventoryId, item.Id, amount))
				{
					FarmingController.StopFarming(player);
					player.Notify("Farming", "Deine Taschen sind voll!", NotificationType.ERROR);
					return;
				}
				player.Notify("Farming", $"+{amount} {item.Name}", Core.Enums.NotificationType.INFO);
			}
		}

		[EveryMinute]
		public static void SpotTick()
		{
			foreach(var spot in FarmingController.CachedFarmingSpots)
			{
				if (spot.Health > 0) continue;

				FarmingController.EnableSpot(spot);
			}
		}
	}
}