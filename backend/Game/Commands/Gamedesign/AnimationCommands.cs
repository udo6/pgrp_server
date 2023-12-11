using Core;
using Core.Attribute;
using Core.Entities;

namespace Game.Commands.Gamedesign
{
	public static class AnimationCommands
	{
		[Command("playanim")]
		public static void PlayAnimation(RPPlayer player, string dict, string name, int flags)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			player.PlayAnimation(dict, name, flags);
		}

		[Command("stopanim")]
		public static void StopAnimation(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			player.StopAnimation();
		}
	}
}