using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Wardrobe;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class WardrobeCommands
	{
		[Command("createwardrobe")]
		public static void CreateWardrobe(RPPlayer player, int owner, int ownerType, int dimension)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var wardrobe = new WardrobeModel(pos.Id, owner, (OwnerType)ownerType, dimension);
			WardrobeService.Add(wardrobe);
			WardrobeController.LoadWardrobe(wardrobe);
		}

		[Command("addwardrobeitem")]
		public static void AddWardrobeItem(RPPlayer player, int ownerId, int ownerType, string label, int component, int drawable, int texture, int dlc, int prop, int gender)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var item = new WardrobeItemModel(ownerId, (OwnerType)ownerType, label.Replace('_', ' '), gender, component, drawable, texture, (uint)dlc, Convert.ToBoolean(prop));
			WardrobeService.AddItem(item);
		}
	}
}