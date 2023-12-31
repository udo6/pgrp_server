using Core.Attribute;
using Core.Entities;
using Database.Services;

namespace Game.Commands.Admin
{
	public static class PositionCommands
	{
		[Command("gotopos")]
		public static void GotoPos(RPPlayer player, int posId)
		{
			if (player.AdminRank < Core.Enums.AdminRank.ADMINISTRATOR) return;

			var pos = PositionService.Get(posId);
			if (pos == null) return;

			player.SetPosition(pos.Position);
		}
	}
}