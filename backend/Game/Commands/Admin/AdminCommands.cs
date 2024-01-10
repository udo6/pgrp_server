using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
using Game.Controllers;
using Game.Modules;

namespace Game.Commands.Admin
{
	public static class AdminCommands
	{
		[Command("setadmin")]
		public static void SetPlayerAdmin(RPPlayer player, string targetName, int rank)
		{
			if (player.AdminRank < AdminRank.SUPERADMIN || rank >= (int)player.AdminRank) return;

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
			if (player.AdminRank < AdminRank.SUPERADMIN) return;

			player.Streamed = !player.Streamed;
			player.Visible = player.Streamed;
		}

		[Command("a", true)]
		public static void AdminChat(RPPlayer player, string message)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			AdminController.BroadcastTeam("Admin-Chat", $"{player.Name}: {message}", NotificationType.INFO, AdminRank.ADMINISTRATOR);
		}

		[Command("t", true)]
		public static void TeamChat(RPPlayer player, string message)
		{
			if (player.AdminRank < AdminRank.GUIDE) return;

			AdminController.BroadcastTeam("Team-Chat", $"{player.Name}: {message}", NotificationType.INFO, AdminRank.GUIDE);
		}

		[Command("toggleadmininfo")]
		public static void ToggleAdminInfo(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.GUIDE) return;

			player.AdminNotifications = !player.AdminNotifications;
			player.Notify("Information", $"Du hast deine Admin-Nachrichten {(player.AdminNotifications ? "eingeschaltet" : "ausgeschaltet")}.", NotificationType.INFO);
		}

		[Command("names")]
		public static void ToggleNames(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.SUPERADMIN) return;

			player.Emit("Client:AdminModule:ToggleNames");
		}

		[Command("team")]
		public static void Team(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.GUIDE) return;

			var teamOnline = RPPlayer.All.ToList().Count(x => x.AdminRank > AdminRank.SPIELER);
			player.Notify("Administration", $"Es sind {teamOnline} Team Mitglieder online!", NotificationType.INFO);
		}

		[Command("goback")]
		public static void GoBack(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.SUPPORTER || player.AdminGoBackPosition.X == 0 && player.AdminGoBackPosition.Y == 0) return;

			player.SetPosition(player.AdminGoBackPosition);
			player.AdminGoBackPosition = new(0, 0, 0);
			player.Notify("Administration", "Du hast dich zurück teleportiert!", NotificationType.SUCCESS);
		}

		[Command("restart")]
		public static void Restart(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.SUPERADMIN) return;

			Restarts.Restart();
		}
	}
}