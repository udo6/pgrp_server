using Core.Attribute;
using Core.Entities;
using Database.Services;

namespace Game.Commands.Admin
{
	public static class LoadoutCommands
	{
		[Command("clearloadout")]
		public static void ClearLoadout(RPPlayer player, string targetName)
		{
			if (player.AdminRank < Core.Enums.AdminRank.ADMINISTRATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			target.RemoveAllWeapons(true);
			target.Weapons.Clear();
			LoadoutService.ClearPlayerLoadout(target.DbId);
			player.Notify("Administration", $"Du hast das Loadout von {target.Name} gecleared!", Core.Enums.NotificationType.SUCCESS);
			target.Notify("Administration", $"Dein Loadout wurde von {player.Name} gecleared!", Core.Enums.NotificationType.WARN);
		}
	}
}