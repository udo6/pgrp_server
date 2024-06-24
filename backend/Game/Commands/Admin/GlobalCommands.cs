using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Newtonsoft.Json;

namespace Game.Commands.Admin
{
	public static class GlobalCommands
	{
		[Command("announce", true)]
		public static void Announce(RPPlayer player, string message)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			var data = JsonConvert.SerializeObject(new
			{
				Title = "ADMINISTRATIVE NACHRICHT",
				Message = message,
				Duration = 15000,
                Type = GlobalNotifyType.GLOBAL
            });

			foreach(var target in RPPlayer.All.ToList())
			{
				target.EmitBrowser("Hud:ShowGlobalNotification", data);
			}
		}
	}
}