using Core.Attribute;
using Core.Entities;
using Game.Controllers;

namespace Game.Commands.Player
{
	public static class FFACommands
	{
		[Command("quitffa")]
		public static void QuitFFA(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInFFA) return;

			FFAController.QuitFFA(player);
		}
	}
}