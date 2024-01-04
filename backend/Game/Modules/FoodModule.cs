using Core.Attribute;
using Core.Entities;
using Core.Enums;

namespace Game.Modules
{
	public static class FoodModule
	{
		[EveryMinute]
		public static void OnEveryMinute()
		{
			foreach(var player in RPPlayer.All.ToList())
			{
				if (player == null || !player.Exists || !player.LoggedIn || player.AdminDuty || !player.Alive) continue;

				if (player.Hunger <= 0 && player.Thirst <= 0)
				{
					player.Health -= 3;
					player.Notify("Information", "Du solltest etwas essen!", NotificationType.WARN);
					return;
				}

				player.Hunger--;
				player.Thirst--;
				player.EmitBrowser("Hud:SetFood", player.Hunger, player.Thirst, 110);
			}
		}
	}
}