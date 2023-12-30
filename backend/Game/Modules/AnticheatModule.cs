using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Logs;

namespace Game.Modules
{
	public static class AnticheatModule
	{
		private static Dictionary<uint, int> WeaponDamage = new()
		{
			{ 453432689u, 6 },   // Pistol
			{ 1593441988u, 6 },  // Combat Pistol
			{ 3523564046u, 8 },  // Heavy Pistol
			{ 2578377531u, 10 }, // Pistol50
			{ 3219281620u, 8 },  // Pistol MKII
			{ 3220176749u, 9 },  // Assaultrifle
			{ 2210333304u, 8 },  // Carbinerifle
			{ 2132975508u, 8 },  // Bullpuprifle
			{ 2937143193u, 8 },  // Advancedrifle
			{ 3231910285u, 9 },  // Specialcarbine
			{ 2636060646u, 8 },  // Militaryrifle
			{ 3347935668u, 8 },  // Heavyrifle
		};

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:Anticheat:VehicleEngineToggle", DetectedVehicleEngine);
			Alt.OnClient<RPPlayer, uint>("Server:Anticheat:NoReload", DetectedNoreload);
			Alt.OnClient<RPPlayer, uint, float>("Server:Anticheat:Rapidfire", DetectedRapidfire);
			Alt.OnClient<RPPlayer, uint, string, float>("Server:Anticheat:WeaponModification", DetectedModifiedWeapon);
			Alt.OnClient<RPPlayer, Position>("Server:Anticheat:Teleport", DetectedTeleport);
			Alt.OnClient<RPPlayer, bool>("Server:Anticheat:Godmode", DetectedGodmode);
			Alt.OnClient<RPPlayer, int>("Server:Anticheat:Healkey", DetectedHealkey);
			Alt.OnClient<RPPlayer>("Server:Anticheat:RocketBoost", DetectedRocketBoost);
			Alt.OnClient<RPPlayer>("Server:Anticheat:VehicleParachute", DetectedVehicleParachute);

			Alt.OnWeaponDamage += OnWeaponDamage;
			Alt.OnExplosion += OnExplosion;
		}

		// server ac
		public static void DetectedUnallowedAmmo(RPPlayer player, uint weapon, int ammo, int allowedAmmo)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Unallowed ammo (Weapon: {weapon}, Ammo: {ammo}, Allowed Ammo: {allowedAmmo})");
		}

		public static void DetectedWeapon(RPPlayer player, uint weapon)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Unallowed weapon (Weapon: {weapon})");
		}

		public static void DetectedAttatchment(RPPlayer player, uint weapon, uint attatchment)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Unallowed attatchment (Weapon: {weapon}, Attatchment: {attatchment})");
		}

		// client ac

		private static void DetectedVehicleEngine(RPPlayer player)
		{
			AdminController.BroadcastTeam("Anticheat", $"{player.Name} hat den Motor von einem Fahrzeug eingeschaltet!", Core.Enums.NotificationType.WARN, Core.Enums.AdminRank.ADMINISTRATOR);
		}

		private static void DetectedNoreload(RPPlayer player, uint weapon)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Weapon no reload (Weapon: {weapon})");
		}

		private static void DetectedRapidfire(RPPlayer player, uint weapon, float time)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Weapon rapidfire (Weapon: {weapon} Time: {time})");
		}

		private static void DetectedModifiedWeapon(RPPlayer player, uint weapon, string label, float value)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Weapon modification (Weapon: {weapon} Label: {label} Value: {value})");
		}

		private static void DetectedTeleport(RPPlayer player, Position position)
		{
			var dist = player.Position.Distance(position);
			if (player.LastPositionChange.AddSeconds(5) >= DateTime.Now || dist <= 15) return;

			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Teleport (Distance: {dist})");
		}

		private static void DetectedGodmode(RPPlayer player, bool state)
		{
			if (player.Invincible == state) return;

			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Godmode (Invincible: {player.Invincible} Allowed: {state})");
		}

		private static void DetectedHealkey(RPPlayer player, int health)
		{
			if (player.LastHealthChange.AddSeconds(5) >= DateTime.Now || player.Health + player.Armor <= player.AllowedHealth) return;

			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Healkey (Health: {player.Health + player.Armor} Allowed Health: {health})");
		}

		private static void DetectedRocketBoost(RPPlayer player)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Vehicle rocketboost (Vehicle: {player.Vehicle.Model})");
		}

		private static void DetectedVehicleParachute(RPPlayer player)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Vehicle parachute (Vehicle: {player.Vehicle.Model})");
		}

		private static WeaponDamageResponse OnWeaponDamage(IPlayer _player, IEntity target, uint weapon, ushort damage, Position shotOffset, BodyPart bodyPart)
		{
			var player = (RPPlayer)_player;

			if (target.Type == BaseObjectType.Player)
			{
				var targetPlayer = ((RPPlayer)target);

				targetPlayer.Emit("Client:AnticheatModule:SetHealth", targetPlayer.Health + targetPlayer.Armor);

				LogService.LogDamage(player.DbId, targetPlayer.DbId, weapon, damage, (int)bodyPart);
			}

			if (WeaponDamage.ContainsKey(weapon))
			{
				var allowedDamage = WeaponDamage[weapon];
				if (damage > allowedDamage)
				{
					var account = AccountService.Get(player.DbId);
					if (account == null) return false;

					account.BannedUntil = DateTime.Now.AddYears(10);
					account.BanReason = "Cheating";
					AccountService.Update(account);

					LogService.LogPlayerBan(player.DbId, 0, $"Damage modifier (Weapon: {weapon} Damage: {damage} Allowed Damage: {allowedDamage})");
					player.Kick("Du wurdest gebannt! Grund: Cheating");
					return false;
				}
			}

			return true;
		}

		private static bool OnExplosion(IPlayer _player, ExplosionType explosionType, Position position, uint explosionFx, IEntity targetEntity)
		{
			var player = (RPPlayer) _player;

			if(explosionType == ExplosionType.Unknown || explosionType == ExplosionType.Car) return false;

			if(explosionType != ExplosionType.Flare && explosionType != ExplosionType.Snowball)
			{
				player.ExplosionsCaused++;
				if (player.ExplosionsCaused > 10)
				{
					AdminController.BroadcastTeam("Anticheat", $"{player.Name} hat {player.ExplosionsCaused} Explosionen verursacht!", Core.Enums.NotificationType.WARN, Core.Enums.AdminRank.MODERATOR);
				}
			}

			if (explosionType == ExplosionType.GrenadeLauncher)
			{
				var account = AccountService.Get(player.DbId);
				if (account == null) return false;

				account.BannedUntil = DateTime.Now.AddYears(10);
				account.BanReason = "Cheating";
				AccountService.Update(account);

				LogService.LogPlayerBan(player.DbId, 0, $"SKRIPT Explosive Ammo (Grenadelauncher)");
				player.Kick("Du wurdest gebannt! Grund: Cheating");
				return false;
			}

			LogService.LogExplosion(player.DbId, (int)explosionType);

			return true;
		}
	}
}
