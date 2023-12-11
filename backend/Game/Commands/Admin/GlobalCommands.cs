using Core.Attribute;
using Core.Entities;
using Newtonsoft.Json;

namespace Game.Commands.Admin
{
	public static class GlobalCommands
	{
		[Command("announce", true)]
		public static void Announce(RPPlayer player, string message)
		{
			if (player.AdminRank < Core.Enums.AdminRank.ADMINISTRATOR) return;

			var data = JsonConvert.SerializeObject(new
			{
				Title = "ADMINISTRATIVE NACHRICHT",
				Message = message,
				Duration = 15000
			});

			foreach(var target in RPPlayer.All.ToList())
			{
				target.EmitBrowser("Hud:ShowGlobalNotification", data);
			}
		}
	}
}