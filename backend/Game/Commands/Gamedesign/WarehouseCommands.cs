using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Warehouse;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class WarehouseCommands
	{
		[Command("createwarehouse")]
		public static void CreateWarehouse(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var warehouse = new WarehouseModel(pos.Id, Core.Enums.WarehouseType.SMALL, 0, 0, 0, 0);
			WarehouseService.Add(warehouse);
			WarehouseController.LoadWarehouse(warehouse);
		}
	}
}