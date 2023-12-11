using Core.Attribute;
using Core.Entities;
using Game.Controllers;

namespace Game.Commands.Admin
{
	public static class TeamCommands
	{
		[Command("setteam")]
		public static void SetTeam(RPPlayer player, string targetName, int team, int rank, int leader, int storage, int bank)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			PlayerController.SetPlayerTeam(target, team, rank, Convert.ToBoolean(leader), Convert.ToBoolean(storage), Convert.ToBoolean(bank));
		}
	}
}