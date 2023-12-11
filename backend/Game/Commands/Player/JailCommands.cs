using Core.Attribute;
using Core.Entities;
using Core.Models.NativeMenu;

namespace Game.Commands.Player
{
	public static class JailCommands
	{
		[Command("jailtime")]
		public static void Jailtime(RPPlayer player)
		{
			if (player.Jailtime < 1) return;

			player.Notify("Information", $"Du bist noch für {player.Jailtime} Hafteinheiten im Gefängnis!", Core.Enums.NotificationType.INFO);
		}

		[Command("jail")]
		public static void Jail(RPPlayer player)
		{
			if ((player.TeamId < 1 || player.TeamId > 2) && player.TeamId != 5) return;

			var list = new List<NativeMenuItem>();
			foreach (var target in RPPlayer.All.ToList())
			{
				if (target == null || !target.Exists || target.Jailtime < 1) continue;

				list.Add(new($"{target.Name} - {target.Jailtime}HE", false, "Server:Jail:AccessInmate", target.DbId));
			}

			player.ShowNativeMenu(true, new("Gefängnis Insassen", list));
		}
	}
}