using Core.Attribute;
using Core.Entities;

namespace Game.Commands.Player
{
	public static class PlayerCommands
	{
		[Command("ooc", true)]
		public static void OutOfCharacter(RPPlayer player, string message)
		{
			foreach(var target in RPPlayer.All.ToList())
			{
				if (target.Position.Distance(player.Position) > 20f) continue;

				target.Notify("OOC Chat", $"{player.Name}: {message}", Core.Enums.NotificationType.INFO);
			}
		}
	}
}