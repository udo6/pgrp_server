using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Database.Services;

namespace Game.Modules
{
	public static class WeaponModule
	{
		// default clip
		private static Dictionary<uint, List<uint>> AttatchmentWhitelist = new()
		{
			{ 453432689u, new(){ 0xFED0FD71 } },   // Pistol
			{ 1593441988u, new(){ 0x721B079 } },   // Combat Pistol
			{ 3523564046u, new(){ 0xD4A969A } },   // Heavy Pistol
			{ 2578377531u, new(){ 0x2297BE19 } },  // Pistol50
			{ 3219281620u, new(){ 0x94F42D62 } },  // Pistol MKII
			{ 3220176749u, new(){ 0xBE5EEA16 } },  // Assaultrifle
			{ 2210333304u, new(){ 0x9FBE33EC } },  // Carbinerifle
			{ 2937143193u, new(){ 0xFA8FA10F } },  // Advancedrifle
			{ 2132975508u, new(){ 0xC5A12F80 } },  // Bullpuprifle
			{ 3231910285u, new(){ 0xC6C7E581 } },  // Specialcarbine
			{ 2636060646u, new(){ 0x2D46D83B, 0x6B82F395 } },  // Militaryrifle
			{ 3347935668u, new(){ 1525977990 } },  // Heavyrifle
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

			if(newWeapon != 2725352035)
			{
				var weapon = player.Weapons.FirstOrDefault(x => x.Hash == newWeapon);
				if (weapon == null)
				{
					AnticheatModule.DetectedWeapon(player, newWeapon);
					return;
				}

				/*player.GetCurrentWeaponComponents(out var components);
				var unallowedComponent = components.FirstOrDefault(x => !weapon.Components.Contains(x) && (!AttatchmentWhitelist.ContainsKey(newWeapon) || AttatchmentWhitelist[newWeapon] != x));
				if (unallowedComponent > 0)
				{
					AnticheatModule.DetectedAttatchment(player, newWeapon, unallowedComponent);
					return;
				}*/
			}

			if(oldWeapon != 2725352035)
			{
				var loadout = LoadoutService.GetLoadout(player.DbId, oldWeapon);
				if (loadout == null) return;

				var ammo = player.GetWeaponAmmo(oldWeapon);
				if (ammo == loadout.Ammo) return;

				/*if (ammo > loadout.Ammo)
				{
					AnticheatModule.DetectedUnallowedAmmo(player, oldWeapon, ammo, loadout.Ammo);
					return;
				}*/

				loadout.Ammo = ammo;
				LoadoutService.Update(loadout);
			}
		}
	}
}