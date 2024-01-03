using Core.Attribute;
using Core.Entities;
using Database.Services;
using Newtonsoft.Json;

namespace Game.Commands.Admin
{
	public static class SupportCommands
	{
		[Command("laptop")]
		public static void OpenLaptop(RPPlayer player)
		{
			if (player.AdminRank < Core.Enums.AdminRank.ADMINISTRATOR) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.PlayAnimation(Core.Enums.AnimationType.LAPTOP);
			player.ShowComponent("Laptop", true, JsonConvert.SerializeObject(new
			{
				Name = player.Name,
				Team = account.TeamId,
				TeamLeader = account.TeamAdmin,
				Admin = player.AdminRank
			}));
		}
	}
}
