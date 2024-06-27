using AltV.Net.Data;
using AltV.Net;
using Core.Attribute;
using Core.Entities;
using AltV.Net.Elements.Entities;

namespace Game.Commands.Gamedesign
{
	public static class IPLCommands
	{
		[Command("loadipl")]
		public static void LoadIPL(RPPlayer player, string ipl)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			player.LoadIPL(ipl);
		}

		[Command("unloadipl")]
		public static void UnloadIPL(RPPlayer player, string ipl)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			player.UnloadIPL(ipl);
		}
	}
}