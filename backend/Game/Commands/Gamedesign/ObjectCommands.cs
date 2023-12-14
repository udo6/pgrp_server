using AltV.Net;
using Core.Attribute;
using Core.Entities;

namespace Game.Commands.Gamedesign
{
	public static class ObjectCommands
	{
		[Command("createobj")]
		public static void CreateObject(RPPlayer player, int hash, int frozen)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var obj = Alt.CreateObject((uint)hash, player.Position, player.Rotation);
			obj.Frozen = frozen > 0;
			player.Notify("Gamedesign", $"ID: {obj.Id}", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("deleteobj")]
		public static void DeleteObj(RPPlayer player, int id)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var obj = Alt.GetAllNetworkObjects().FirstOrDefault(x => x.Id == id);
			if (obj == null) return;

			obj.Destroy();
		}
	}
}