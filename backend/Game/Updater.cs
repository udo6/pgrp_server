using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Logs;

namespace Game
{
	public static class Updater
	{
		[EveryMinute]
		public static void UpdateVehicles()
		{
			foreach(var vehicle in RPVehicle.All.ToList())
			{
				if (vehicle.DbId < 1) continue;

				var pos = PositionService.Get(vehicle.PositionId);
				if (pos == null) continue;

				pos.Position = vehicle.Position;
				pos.Rotation = vehicle.Rotation;
				PositionService.Update(pos);
			}
		}

		[EveryMinute]
		public static void UpdatePlayers()
		{
			foreach(var player in RPPlayer.All.ToList())
			{
				if(player.DbId < 1) continue;

				if (player.DamageLogs.Count > 0)
				{
					LogService.LogDamage(player.DamageLogs);
					player.DamageLogs.Clear();
				}

				var account = AccountService.Get(player.DbId);
				if (account == null) continue;

				account.Health = player.Health;
				account.Armor = player.Armor;
				account.Alive = player.Alive;
				account.InjuryType = player.InjuryType;
				AccountService.Update(account);

				var pos = PositionService.Get(player.PositionId);
				if (pos == null) continue;

				if (player.InInterior)
					pos.Position = player.OutsideInteriorPosition;
				else
					pos.Position = player.Position;
				pos.Rotation = player.Rotation;
				PositionService.Update(pos);
			}
		}
	}
}
