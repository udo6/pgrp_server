using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Tattoo;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class TattooCommands
	{
		[Command("createtattooshop")]
		public static void CreateTattooShop(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var model = new TattooShopModel(pos.Id);
			TattooService.AddShop(model);
			TattooController.LoadTattooShop(model);
		}

		[Command("addtattoo")]
		public static void AddTattoo(RPPlayer player, int shopId, int collection, int overlay, int category, string label, int price)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var model = new TattooShopItemModel(shopId, (uint)collection, (uint)overlay, category, label.Replace('_', ' '), price);
			TattooService.AddItem(model);
		}
	}
}