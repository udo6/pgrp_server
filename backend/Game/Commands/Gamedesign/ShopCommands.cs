using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Shop;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class ShopCommands
	{
		[Command("createshop")]
		public static void CreateShop(RPPlayer player, string name, int owner, int type)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var shop = new ShopModel(name.Replace('_', ' '), pos.Id, 0, (ShopType)type, owner);
			ShopService.Add(shop);
			ShopController.LoadShop(shop);
		}

		[Command("addshopped")]
		public static void AddShopPed(RPPlayer player, int shopId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var shop = ShopService.Get(shopId);
			if (shop == null) return;

			shop.PedPositionId = pos.Id;
			ShopService.Update(shop);
		}

		[Command("addshopitem")]
		public static void AddShopItem(RPPlayer player, int shopId, int itemId, int minPrice, int maxPrice, int minRank)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var item = new ShopItemModel(shopId, itemId, 0, minPrice, maxPrice, minRank);
			ShopService.AddItem(item);
		}
	}
}