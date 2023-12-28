using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Game.Modules
{
	public static class AnticheatModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, uint, string, float>("Server:Anticheat:WeaponModification", DetectedModifiedWeapon);
			Alt.OnClient<RPPlayer, RPPlayer, int>("Server:Anticheat:GodmodeTarget", DetectedGodmodeTarget);
			Alt.OnClient<RPPlayer, uint, int, int>("Server:Anticheat:DamageModifier", DetectedDamageModifier);
			Alt.OnClient<RPPlayer, Position>("Server:Anticheat:Teleport", DetectedTeleport);
			Alt.OnClient<RPPlayer, bool>("Server:Anticheat:Godmode", DetectedGodmode);
			Alt.OnClient<RPPlayer, int>("Server:Anticheat:Healkey", DetectedHealkey);
			Alt.OnClient<RPPlayer>("Server:Anticheat:RocketBoost", DetectedRocketBoost);
			Alt.OnClient<RPPlayer>("Server:Anticheat:VehicleParachute", DetectedVehicleParachute);
			Alt.OnClient<RPPlayer, uint>("Server:Anticheat:Weapon", DetectedWeapon);

			Alt.OnWeaponDamage += OnWeaponDamage;
			Alt.OnExplosion += OnExplosion;
		}

		private static void DetectedModifiedWeapon(RPPlayer player, uint weapon, string label, float value)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Weapon modification (Weapon: {weapon} Label: {label} Value: {value})");
			player.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static void DetectedGodmodeTarget(RPPlayer player, RPPlayer target, int allowedHealth)
		{
			if (target == null || target.Invincible || !player.Streamed || !target.Streamed || target.LastGodmodeChange.AddSeconds(5) >= DateTime.Now) return;

			if (player.Position.Distance(target.Position) > 200) return;

			var account = AccountService.Get(target.DbId);
			if (account == null) return;

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(target.DbId, player.DbId, $"[ANTICHEAT] Godmode (Health: {player.Health + player.Armor} Allowed Health: {allowedHealth})");
			target.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static void DetectedDamageModifier(RPPlayer player, uint weapon, int damage, int allowedDamage)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Damage modifier (Weapon: {weapon} Damage: {damage} Allowed Damage: {allowedDamage})");
			player.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static void DetectedTeleport(RPPlayer player, Position position)
		{
			if (player.LastPositionChange.AddSeconds(5) >= DateTime.Now || player.Position.Distance(position) <= 15) return;

			//var account = AccountService.Get(player.DbId);
			//if (account == null) return;

			//account.BannedUntil = DateTime.Now.AddYears(10);
			//account.BanReason = "Cheating";
			//AccountService.Update(account);

			Console.WriteLine($"[ANTICHEAT] {player.Name} has been banned for Teleport");
			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Teleport");
			//player.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static void DetectedGodmode(RPPlayer player, bool state)
		{
			if (player.Invincible == state) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Godmode (Invincible: {player.Invincible} Allowed: {state})");
			player.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static void DetectedHealkey(RPPlayer player, int health)
		{
			if (player.LastHealthChange.AddSeconds(5) >= DateTime.Now || player.Health + player.Armor <= player.AllowedHealth) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Healkey (Health: {player.Health + player.Armor} Allowed Health: {health})");
			player.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static void DetectedRocketBoost(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Vehicle rocketboost (Vehicle: {player.Vehicle.Model})");
			player.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static void DetectedVehicleParachute(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Vehicle parachute (Vehicle: {player.Vehicle.Model})");
			player.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static void DetectedWeapon(RPPlayer player, uint weapon)
		{
			if (player.Weapons.Any(x => x == weapon)) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Unallowed weapon (Weapon: {weapon})");
			player.Kick("Du wurdest gebannt! Grund: Cheating");
		}

		private static WeaponDamageResponse OnWeaponDamage(IPlayer player, IEntity target, uint weapon, ushort damage, Position shotOffset, BodyPart bodyPart)
		{
			if (target.Type == BaseObjectType.Player)
			{
				var targetPlayer = ((RPPlayer)target);

				targetPlayer.Emit("Client:AnticheatModule:SetHealth", targetPlayer.Health + targetPlayer.Armor);
			}

			return true;
		}

		private static bool OnExplosion(IPlayer _player, ExplosionType explosionType, Position position, uint explosionFx, IEntity targetEntity)
		{
			var player = (RPPlayer) _player;

			if(explosionType != ExplosionType.Flare && explosionType != ExplosionType.Snowball)
			{
				player.ExplosionsCaused++;
				if (player.ExplosionsCaused > 20)
				{
					var account = AccountService.Get(player.DbId);
					if (account == null) return false;

					account.BannedUntil = DateTime.Now.AddYears(10);
					account.BanReason = "Cheating";
					AccountService.Update(account);

					LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Too many explosions caused (Explosions: {player.ExplosionsCaused}, Type: {explosionType})");
					player.Kick("Du wurdest gebannt! Grund: Cheating");
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

				LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] SKRIPT Explosive Ammo (Grenadelauncher)");
				player.Kick("Du wurdest gebannt! Grund: Cheating");
				return false;
			}

			LogService.LogExplosion(player.DbId, (int)explosionType);

			return true;
		}
	}
}
