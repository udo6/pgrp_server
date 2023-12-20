using Core.Attribute;
using Core.Entities;

namespace Game.Commands.Player
{
	public static class PlayerCommands
	{
		private static List<string> BlacklistedWords = new()
		{
			"nigga",
			"nigger",
			"hurensohn",
		};

		[Command("ooc", true)]
		public static void OutOfCharacter(RPPlayer player, string message)
		{
			if(BlacklistedWords.Any(x => x == message.ToLower()))
			{
				player.Notify("Information", "Du hast versucht ein verbotenes Wort zu schreiben", Core.Enums.NotificationType.ERROR);
				return;
			}

			foreach(var target in RPPlayer.All.ToList())
			{
				if (target.Position.Distance(player.Position) > 20f) continue;

				target.Notify("OOC Chat", $"{player.Name}: {message}", Core.Enums.NotificationType.INFO);
			}
		}
	}
}