﻿using Core.Entities;
using Core.Enums;

namespace Game.Controllers
{
	public static class AdminController
	{
		public static void BroadcastTeam(string title, string message, NotificationType type, AdminRank minRank = AdminRank.SUPPORTER)
		{
			foreach(var player in RPPlayer.All.ToList())
			{
				if (player.AdminRank < minRank || !player.AdminNotifications) continue;

				player.Notify(title, message, type, 10000);
			}
		}

		public static void BroadcastHighteam(string title, string message, NotificationType type)
		{
			foreach (var player in RPPlayer.All.ToList())
			{
				if (player.AdminRank < AdminRank.SUPERADMIN) continue;

				player.Notify(title, message, type, 10000);
			}
		}
	}
}
