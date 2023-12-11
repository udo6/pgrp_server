﻿using Core.Entities;
using Core.Enums;

namespace Game.Controllers
{
	public static class AdminController
	{
		public static void BroadcastTeam(string title, string message, NotificationType type)
		{
			foreach(var player in RPPlayer.All.ToList())
			{
				if (player.AdminRank < AdminRank.SUPPORTER) continue;

				player.Notify(title, message, type);
			}
		}

		public static void BroadcastHighteam(string title, string message, NotificationType type)
		{
			foreach (var player in RPPlayer.All.ToList())
			{
				if (player.AdminRank < AdminRank.SUPERADMINISTRATOR) continue;

				player.Notify(title, message, type);
			}
		}
	}
}
