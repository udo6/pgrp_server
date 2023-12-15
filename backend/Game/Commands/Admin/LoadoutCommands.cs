using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Account;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Admin
{
	public static class LoadoutCommands
	{
		[Command("giveweapon")]
		public static void GiveWeapon(RPPlayer player, int weapon, int ammo)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var loadout = new LoadoutModel(player.DbId, (uint)weapon, ammo, Core.Enums.LoadoutType.DEFAULT);
			LoadoutService.Add(loadout);
			PlayerController.ApplyPlayerLoadout(player);
		}

		[Command("giveattatchment")]
		public static void GiveAttachment(RPPlayer player, int hash)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var loadout = LoadoutService.Get(player.CurrentWeapon);
			if (loadout == null) return;

			var attatchment = new LoadoutAttatchmentModel(loadout.Id, (uint)hash);
			LoadoutService.AddAttatchment(attatchment);
			PlayerController.ApplyPlayerLoadout(player);
		}
	}
}