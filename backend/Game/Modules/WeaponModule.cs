using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Enums;
using Core.Attribute;
using Core.Entities;
using Database.Services;

namespace Game.Modules
{
	public static class WeaponModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnPlayerWeaponChange += OnWeaponSwitch;
		}

		private static void OnWeaponSwitch(IPlayer _player, uint oldWeapon, uint newWeapon)
		{
			var player = (RPPlayer)_player;

			if (oldWeapon == 2725352035u) return;

			var loadout = LoadoutService.GetLoadout(player.DbId, oldWeapon);
			if (loadout == null) return;

			var ammo = player.GetWeaponAmmo(oldWeapon);
			if (loadout.Ammo == ammo) return;

			if (ammo > loadout.Ammo)
			{
				Console.WriteLine($"[ANTICHEAT] {player.Name}: Ammo was more than allowed! ({ammo}) ({loadout.Ammo})");
				return;
			}

			loadout.Ammo = ammo;
			LoadoutService.Update(loadout);
		}
	}
}