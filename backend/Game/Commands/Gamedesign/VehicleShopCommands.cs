using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.VehicleShop;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class VehicleShopCommands
	{
		[Command("createvehicleshop")]
		public static void CreateVehicleShop(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var shop = new VehicleShopModel(pos.Id, pos.Id);
			VehicleShopService.Add(shop);
			VehicleShopController.LoadVehicleShop(shop);
			player.Notify("Information", $"ID: {shop.Id}", NotificationType.INFO);
		}

		[Command("addvehicleshopitem")]
		public static void AddVehicleShopItem(RPPlayer player, int shopId, int baseId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN || !player.IsInVehicle) return;

			var pos = new PositionModel(player.Vehicle.Position, player.Vehicle.Rotation);
			PositionService.Add(pos);

			var item = new VehicleShopItemModel(shopId, baseId, pos.Id);
			VehicleShopService.AddItem(item);
			VehicleShopController.LoadVehicleShopItem(item, VehicleService.GetAllBases());
		}

		[Command("addvehicleshopspawn")]
		public static void AddVehicleShopSpawn(RPPlayer player, int shopId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN || !player.IsInVehicle) return;

			var pos = new PositionModel(player.Vehicle.Position, player.Vehicle.Rotation);
			PositionService.Add(pos);

			var spawn = new VehicleShopSpawnModel(shopId, pos.Id);
			VehicleShopService.AddSpawn(spawn);
		}
	}
}