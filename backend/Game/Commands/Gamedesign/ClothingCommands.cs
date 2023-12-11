using Core;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class ClothingCommands
	{
		[Command("setclothes")]
		public static void SetPlayerClothes(RPPlayer player, int slot, int drawable, int texture, int dlc = 0)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			PlayerController.SetClothes(player, slot, drawable, texture, (uint)dlc, true);
		}

		[Command("setprop")]
		public static void SetPlayerProp(RPPlayer player, int slot, int drawable, int texture, int dlc = 0)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			PlayerController.SetProps(player, slot, drawable, texture, (uint)dlc, true);
		}
	}
}