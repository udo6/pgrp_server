using Core.Attribute;
using Core.Entities;
using Database.Services;

namespace Game.Commands.Admin
{
	public static class GangwarCommands
	{
		[Command("resetgwcooldown")]
		public static void ResetGangwarCooldown(RPPlayer player, int gangwarId)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var gw = GangwarService.Get(gangwarId);
			if (gw == null) return;

			gw.LastAttack = DateTime.Now.AddYears(-1);
			GangwarService.Update(gw);
		}
	}
}