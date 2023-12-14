using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.GasStation;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class GasStationCommands
	{
		[Command("creategasstation")]
		public static void CreateGasStation(RPPlayer player, string name, int minPrice, int maxPrice)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var model = new GasStationModel(name.Replace('_', ' '), pos.Id, minPrice, maxPrice);
			GasStationService.Add(model);
			GasStationController.LoadGasStation(model);
		}
	}
}