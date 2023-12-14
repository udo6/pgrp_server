using Core;
using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.ClothesShop;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class ClothesShopCommands
	{
		[Command("createclothesshop")]
		public static void CreateClothesShop(RPPlayer player, string name)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var shop = new ClothesShopModel(name.Replace('_', ' '), pos.Id, 0);
			ClothesShopService.Add(shop);
			ClothesShopController.LoadClothesShop(shop);
		}

		[Command("addclothes")]
		public static void CreateClothesShop(RPPlayer player, string name, int slot, int drawable, int texture, int dlc, int prop, int shopId, int price, int gender)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var item = new ClothesShopItemModel(name.Replace('_', ' '), slot, drawable, texture, (uint)dlc, Convert.ToBoolean(prop), shopId, price, gender);
			ClothesShopService.AddItem(item);
		}
	}
}