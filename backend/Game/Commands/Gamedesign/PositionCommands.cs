using Core;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Newtonsoft.Json;

namespace Game.Commands.Gamedesign
{
	public static class PositionCommands
	{
		[Command("pos")]
		public static void GetPosition(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			Logger.LogInfo($"Position von {player.Name}: {JsonConvert.SerializeObject(player.Position)} | {JsonConvert.SerializeObject(player.Rotation)}");
		}

		[Command("tppos")]
		public static void TeleportToPosition(RPPlayer player, float x, float y, float z)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			player.SetPosition(new(x, y, z));
		}
	}
}