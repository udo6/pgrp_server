using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Database.Services;

namespace Game.Modules
{
	public static class WeaponModule
	{
		private static List<uint> AttatchmentWhitelist = new()
		{
			0xFED0FD71, // Pistol DEFAULT MAG
			0x721B079, // Combat Pistol DEFAULT MAG
			0xD4A969A, // Heavy Pistol DEFAULT MAG
			0x2297BE19, // Pistol50 DEFAULT MAG
			0x94F42D62, // Pistol MKII DEFAULT MAG
			0xBE5EEA16, // Assaultrifle DEFAULT MAG
			0x9FBE33EC, // Carbinerifle DEFAULT MAG
			0xFA8FA10F, // Advancedrifle DEFAULT MAG
			0xC5A12F80, // Bullpuprifle DEFAULT MAG
			0xC6C7E581, // Specialcarbine DEFAULT MAG
			0x2D46D83B, // Militaryrifle DEFAULT MAG
			0x6B82F395, // Militaryrifle DEFAULT SCOPE
			1525977990, // Heavyrifle DEFAULT MAG
		};

		[Initialize]
		public static void Initialize()
		{
			Alt.OnPlayerWeaponChange += OnWeaponSwitch;
		}

		private static void OnWeaponSwitch(IPlayer _player, uint oldWeapon, uint newWeapon)
		{
			var player = (RPPlayer)_player;
			if (player.IsGangwar || player.IsInFFA) return;

			if(newWeapon > 0 && newWeapon != 2725352035)
			{
				var playerWeapon = player.GetWeapons().FirstOrDefault(x => x.Hash == newWeapon);

				var weapon = player.Weapons.FirstOrDefault(x => x.Hash == newWeapon);
				if (weapon == null)
				{
					AnticheatModule.DetectedWeapon(player, newWeapon);
					return;
				}

				var unallowedComponent = playerWeapon.Components.FirstOrDefault(x => !weapon.Components.Contains(x) && !AttatchmentWhitelist.Any(e => e == x));
				if (unallowedComponent > 0)
				{
					AnticheatModule.DetectedAttatchment(player, newWeapon, unallowedComponent);
					return;
				}
			}

			if(oldWeapon != 2725352035)
			{
				var loadout = LoadoutService.GetLoadout(player.DbId, oldWeapon);
				if (loadout == null) return;

				var ammo = player.GetWeaponAmmo(oldWeapon);
				if (ammo == loadout.Ammo) return;

				if (ammo > loadout.Ammo)
				{
					AnticheatModule.DetectedUnallowedAmmo(player, oldWeapon, ammo, loadout.Ammo);
					return;
				}

				loadout.Ammo = ammo;
				LoadoutService.Update(loadout);
			}
		}
	}
}