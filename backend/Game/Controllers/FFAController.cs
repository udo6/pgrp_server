using Core.Entities;
using Database.Models;
using Database.Services;
using Game.Modules;

namespace Game.Controllers
{
	public static class FFAController
	{
		private static Random Random = new Random();

		public static void JoinFFA(RPPlayer player, int ffaId)
		{
			player.FFAId = ffaId;
			player.InInterior = true;
			player.OutsideInteriorPosition = player.Position;
			player.SetDimension(100000 + ffaId);
			SpawnPlayer(player);
			GiveWeapons(player, ffaId);
		}

		public static void QuitFFA(RPPlayer player)
		{
			player.FFAId = 0;
			player.InInterior = false;
			player.SetDimension(0);
			player.Spawn(FFAModule.Position.Position, 0);
			PlayerController.ApplyPlayerLoadout(player);
		}

		public static void SpawnPlayer(RPPlayer player)
		{
			var pos = GetSpawn(player.FFAId);
			if (pos == null) return;

			player.Spawn(player.Position, 0);
			player.SetPosition(pos.Position);
			player.Rotation = pos.Rotation;
			player.SetHealth(200);
			player.SetArmor(100);
		}

		private static void GiveWeapons(RPPlayer player, int ffaId)
		{
			var weapons = FFAService.GetWeapons(ffaId);

			player.RemoveAllWeapons(true);
			player.Weapons.Clear();

			foreach (var weapon in weapons)
				player.AddWeapon(weapon.WeaponHash, 9999, true);
		}

		private static PositionModel? GetSpawn(int ffaId)
		{
			var spawns = FFAService.GetSpawns(ffaId);
			var pos = PositionService.Get(spawns[Random.Next(0, spawns.Count)].PositionId);
			return pos;
		}
	}
}