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
			Alt.OnClient<RPPlayer>("Server:Anticheat:RocketBoost", DetectedRocketBoost);
			Alt.OnClient<RPPlayer>("Server:Anticheat:VehicleParachute", DetectedVehicleParachute);

			Alt.OnWeaponDamage += OnWeaponDamage;
			Alt.OnExplosion += OnExplosion;
			Alt.OnPlayerEvent += OnPlayerEvent;
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

		// todo: impl
		public static void DetectedGodmode(RPPlayer player, bool state)
		{
			PlayerController.AnticheatBanPlayer(player, DateTime.Now.AddYears(10), $"Godmode (Invincible: {player.Invincible} Allowed: {state})");
		}

		// todo: impl
		public static void DetectedHealkey(RPPlayer player, int allowedHealth)
		{
			var health = player.Health + player.Armor;
			AdminController.BroadcastTeam("Anticheat", $"{player.Name} hat ein Healkey Flag ausgelöst!", Core.Enums.NotificationType.WARN, Core.Enums.AdminRank.MODERATOR);
			LogService.LogPlayerBan(player.DbId, 0, $"[ONLY LOGGED] Healkey (Health: {health} Allowed: {allowedHealth})");
		}

		// client ac

		private static void DetectedVehicleEngine(RPPlayer player)
		{
			AdminController.BroadcastTeam("Anticheat", $"{player.Name} hat den Motor von einem Fahrzeug eingeschaltet!", Core.Enums.NotificationType.WARN, Core.Enums.AdminRank.ADMINISTRATOR);
		}

		private static void DetectedNoreload(RPPlayer player, uint weapon)
		{
			//LogService.LogPlayerBan(player.DbId, 0, $"[ONLY LOGGED] Weapon no reload (Weapon: {weapon})");
			//AdminController.BroadcastTeam("Anticheat", $"Der Spieler {player.Name} hat ein No-Reload Flag ausgelöst!", Core.Enums.NotificationType.WARN, Core.Enums.AdminRank.ADMINISTRATOR);
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

			LogService.LogPlayerBan(player.DbId, 0, $"[ONLY LOGGED] Teleport (Distance: {dist})");
			AdminController.BroadcastTeam("Anticheat", $"Der Spieler {player.Name} hat sich Teleportiert! Distance: {dist}m", Core.Enums.NotificationType.WARN, Core.Enums.AdminRank.ADMINISTRATOR);
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
			if(!player.LoggedIn)
			{
				player.Kick("Du wurdest gekicked!");
				if(target.Type == BaseObjectType.Player)
				{
					var targetPlayer = (RPPlayer)target;
					targetPlayer.Spawn(player.Position, 0);
					PlayerController.SetPlayerAlive(targetPlayer, false);
				}
				return false;
			}

			if (target.Type == BaseObjectType.Player)
			{
				var targetPlayer = (RPPlayer)target;

				if (!targetPlayer.Invincible || targetPlayer.Health <= 100)
				{
					targetPlayer.LastAttackerId = player.DbId;
					targetPlayer.AllowedHealth -= damage;

					var targetHealth = targetPlayer.Health + targetPlayer.Armor - damage;
					if (targetHealth > targetPlayer.AllowedHealth)
					{
						player.AllowedHealth = targetHealth;
						DetectedHealkey(targetPlayer, targetPlayer.AllowedHealth);
						return true;
					}
				}

				LogService.LogDamage(player.DbId, targetPlayer.DbId, weapon, damage, (int)bodyPart);
			}

			if (WeaponDamage.ContainsKey(weapon))
			{
				var maxDamage = WeaponDamage[weapon];
				if (damage > maxDamage)
				{
					var account = AccountService.Get(player.DbId);
					if (account == null) return false;

					account.BannedUntil = DateTime.Now.AddYears(10);
					account.BanReason = "Cheating";
					AccountService.Update(account);

					LogService.LogPlayerBan(player.DbId, 0, $"Damage modifier (Weapon: {weapon} Damage: {damage} Allowed Damage: {maxDamage})");
					player.Kick("Du wurdest gebannt! Grund: Cheating");
					return false;
				}

				var dist = player.Position.Distance(target.Position);
				if (dist > 40 && damage == maxDamage)
				{
					AdminController.BroadcastTeam("Anticheat", $"Magic bullets wurden bei {player.Name} vom System erkannt!", Core.Enums.NotificationType.WARN, Core.Enums.AdminRank.ADMINISTRATOR);
					LogService.LogMagicBullet(player.DbId, weapon, damage, dist);
					return false;
				}
			}

			return true;
		}

		private static bool OnExplosion(IPlayer _player, ExplosionType explosionType, Position position, uint explosionFx, IEntity targetEntity)
		{
			var player = (RPPlayer) _player;
			if (!player.LoggedIn)
			{
				player.Kick("Du wurdest gekicked!");
				if(targetEntity.Type == BaseObjectType.Player)
				{
					var targetPlayer = (RPPlayer)targetEntity;
					targetPlayer.Spawn(player.Position, 0);
					PlayerController.SetPlayerAlive(targetPlayer, false);
				}
				if(targetEntity.Type == BaseObjectType.Vehicle)
				{
					var targetVehicle = (RPVehicle)targetEntity;
					targetVehicle.Repair();
					targetVehicle.Rotation = new();
				}
				return false;
			}

			if (explosionType == ExplosionType.Unknown || explosionType == ExplosionType.GasTank || explosionType == ExplosionType.Propane) return false;

			if(explosionType != ExplosionType.Flare && explosionType != ExplosionType.Snowball)
			{
				player.ExplosionsCaused++;
				if (player.ExplosionsCaused > 20)
				{
					var account = AccountService.Get(player.DbId);
					if (account != null)
					{
						account.BannedUntil = DateTime.Now.AddYears(10);
						account.BanReason = "Cheating";
						AccountService.Update(account);
					}

					LogService.LogPlayerBan(player.DbId, 0, $"Caused too many explosions!");
					player.Kick("Du wurdest gebannt! Grund: Cheating");

					foreach(var target in RPPlayer.All.ToList())
					{
						if (target.Position.Distance(player.Position) > 100f) continue;

						PlayerController.SetPlayerAlive(target, false);
						target.Notify("Anticheat", "Du wurdest wiederbelebt da du durch einen sogenannten Letzten Ritt getötet wurdest!", Core.Enums.NotificationType.SUCCESS);
					}

					foreach(var vehicle in RPVehicle.All.ToList())
					{
						if (vehicle.Position.Distance(player.Position) > 100f) continue;

						vehicle.Repair();
						vehicle.Rotation = new();
					}

					return false;
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

		private static void OnPlayerEvent(IPlayer _player, string eventName, object[] args)
		{
			var player = (RPPlayer)_player;

			if(player.LastServerEvent.AddMilliseconds(300) < DateTime.Now)
			{
				player.ServerEventsExecuted = 0;
			}

			if(player.ServerEventsExecuted > 40)
			{
				player.Kick("Du wurdest gekicked! Grund: Spam");
				return;
			}

			player.ServerEventsExecuted++;
			player.LastServerEvent = DateTime.Now;
		}
	}
}
