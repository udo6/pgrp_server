using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.House;
using Database.Models.Inventory;
using Database.Models.Wardrobe;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class HouseCommands
	{
		[Command("createhouse")]
		public static void CreateHouse(RPPlayer player, int type)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var inventory = new InventoryModel(32, 200f, InventoryType.HOUSE);
			InventoryService.Add(inventory);

			var house = new HouseModel(pos.Id, (HouseType)type, 0, inventory.Id, 0, 0, 0);
			HouseService.Add(house);

			var wardrobe = new WardrobeModel(HouseController.HouseWardrobePositions[house.Type], 0, 0, house.Id);
			WardrobeService.Add(wardrobe);

			house.WardrobeId = wardrobe.Id;
			HouseService.Update(house);

			HouseController.LoadHouse(house);
		}
	}
}