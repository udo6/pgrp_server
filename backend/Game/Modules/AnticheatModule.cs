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

namespace Game.Modules
{
	public static class AnticheatModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, RPPlayer, int>("Server:Anticheat:GodmodeTarget", DetectedGodmodeTarget);
			Alt.OnClient<RPPlayer, uint, int, int>("Server:Anticheat:DamageModifier", DetectedDamageModifier);
			Alt.OnClient<RPPlayer, Position>("Server:Anticheat:Teleport", DetectedTeleport);
			Alt.OnClient<RPPlayer, bool>("Server:Anticheat:Godmode", DetectedGodmode);
			Alt.OnClient<RPPlayer, int>("Server:Anticheat:Healkey", DetectedHealkey);
			Alt.OnClient<RPPlayer>("Server:Anticheat:RocketBoost", DetectedRocketBoost);
			Alt.OnClient<RPPlayer>("Server:Anticheat:VehicleParachute", DetectedVehicleParachute);
			Alt.OnClient<RPPlayer, uint>("Server:Anticheat:Weapon", DetectedWeapon);

			Alt.OnWeaponDamage += OnWeaponDamage;
		}

		private static void DetectedGodmodeTarget(RPPlayer player, RPPlayer target, int allowedHealth)
		{
			if (player.Position.Distance(target.Position) > 200) return;

			var account = AccountService.Get(target.DbId);
			if (account == null) return;

			target.Kick("Du wurdest gebannt! Grund: Cheating");

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(target.DbId, player.DbId, $"[ANTICHEAT] Godmode (Health: {player.Health + player.Armor} Allowed Health: {allowedHealth})");
		}

		private static void DetectedDamageModifier(RPPlayer player, uint weapon, int damage, int allowedDamage)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.Kick("Du wurdest gebannt! Grund: Cheating");

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Damage modifier (Weapon: {weapon} Damage: {damage} Allowed Damage: {allowedDamage})");
		}

		private static void DetectedTeleport(RPPlayer player, Position position)
		{
			if (player.Position.Distance(position) <= 15) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.Kick("Du wurdest gebannt! Grund: Cheating");

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Teleport");
		}

		private static void DetectedGodmode(RPPlayer player, bool state)
		{
			if (player.Invincible == state) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.Kick("Du wurdest gebannt! Grund: Cheating");

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Godmode (Invincible: {player.Invincible} Allowed: {state})");
		}

		private static void DetectedHealkey(RPPlayer player, int health)
		{
			if (player.Health + player.Armor <= health) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.Kick("Du wurdest gebannt! Grund: Cheating");

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Healkey (Health: {player.Health + player.Armor} Allowed Health: {health})");
		}

		private static void DetectedRocketBoost(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.Kick("Du wurdest gebannt! Grund: Cheating");

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Vehicle rocketboost (Vehicle: {player.Vehicle.Model})");
		}

		private static void DetectedVehicleParachute(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.Kick("Du wurdest gebannt! Grund: Cheating");

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Vehicle parachute (Vehicle: {player.Vehicle.Model})");
		}

		private static void DetectedWeapon(RPPlayer player, uint weapon)
		{
			if (player.Weapons.Any(x => x == weapon)) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.Kick("Du wurdest gebannt! Grund: Cheating");

			account.BannedUntil = DateTime.Now.AddYears(10);
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, $"[ANTICHEAT] Unallowed weapon (Weapon: {weapon})");
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
	}
}
