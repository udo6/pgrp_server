using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;

namespace Game.Commands.Admin
{
	public static class AdminCommands
	{
		[Command("setadmin")]
		public static void SetPlayerAdmin(RPPlayer player, string targetName, int rank)
		{
			if (player.AdminRank < AdminRank.SUPERADMIN) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			var account = AccountService.Get(target.DbId);
			if (account == null) return;

			target.AdminRank = (AdminRank)rank;
			target.ApplyAdmin();

			account.AdminRank = (AdminRank)rank;
			AccountService.Update(account);
		}

		[Command("v")]
		public static void Vanish(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.GUIDE || (player.AdminRank < AdminRank.SUPERADMIN && !player.AdminDuty)) return;

			player.Streamed = !player.Streamed;
			player.Visible = player.Streamed;
		}
	}
}