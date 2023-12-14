using Core;
using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Farming;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class FarmingCommands
	{
		[Command("createfarming")]
		public static void CreateFarming(RPPlayer player, int objectHash, int neededItem, int gainItem, int minGain, int maxGain, string dict, string name)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var farming = new FarmingModel((uint)objectHash, neededItem, gainItem, minGain, maxGain, dict, name);
			FarmingService.Add(farming);
		}

		[Command("addfarmingspot")]
		public static void AddFarmingSpot(RPPlayer player, int farmingId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var farming = FarmingService.Get(farmingId);
			if (farming == null) return;

			var farmingSpot = new FarmingSpotModel(farmingId, pos.Id);
			FarmingService.AddSpot(farmingSpot);
			FarmingController.LoadFarmingSpot(farming, farmingSpot);
		}
	}
}