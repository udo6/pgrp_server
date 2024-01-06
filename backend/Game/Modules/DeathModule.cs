using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
using Discord;
using Game.Controllers;
using Logs;

namespace Game.Modules
{
	public static class DeathModule
	{
		private static readonly Position RespawnPosition = new(355.58243f, -596.3604f, 28.757568f);
		private static readonly Dictionary<uint, InjuryType> SpecialInjuries = new()
		{
			{ Weapons.Fall, InjuryType.FALL_DAMAGE },
			{ Weapons.Exhaustion, InjuryType.FALL_DAMAGE },

			{ Weapons.Drowning, InjuryType.DROWN },
			{ Weapons.DrowningInVehicle, InjuryType.DROWN },

			{ Weapons.BarbedWire, InjuryType.SLICE },

			{ Weapons.Fire, InjuryType.FIRE },
			{ Weapons.ElectricFence, InjuryType.FIRE },

			{ Weapons.Grenade, InjuryType.EXPLOSION },
			{ Weapons.Explosion, InjuryType.EXPLOSION },
		};

		private static readonly Dictionary<InjuryType, int> InjurySurviveTime = new()
		{
			{ InjuryType.FALL_DAMAGE, 15 },
			{ InjuryType.PUNCH, 15 },
			{ InjuryType.SLICE, 10 },
			{ InjuryType.DROWN, 5 },
			{ InjuryType.SHOT_LOW, 10 },
			{ InjuryType.VEHICLE, 10 },
			{ InjuryType.SHOT_HIGH, 10 },
			{ InjuryType.FIRE, 8 },
			{ InjuryType.EXPLOSION, 5 }
		};

		[EveryMinute]
		public static void OnEveryMinute()
		{
			foreach(var player in RPPlayer.All.ToList())
			{
				if(player == null || !player.Exists || !player.LoggedIn || player.Alive) continue;

				var account = AccountService.Get(player.DbId);
				if (account == null) continue;

				if (player.IsGangwar)
				{
					if (player.DeathTime.AddMinutes(2) <= DateTime.Now)
					{
						GangwarController.RespawnPlayer(player);
					}

					continue;
				}

				if (player.Coma)
				{
					if(player.ComaTime.AddMinutes(5) <= DateTime.Now)
					{
						player.SetPosition(RespawnPosition);
						PlayerController.SetPlayerAlive(player, true);
					}

					continue;
				}

				if (player.IsInHospital || player.DeathTime.AddMinutes(InjurySurviveTime[player.InjuryType] + (player.Stabilized ? 5 : 0)) > DateTime.Now) continue;

				player.Coma = true;
				player.ComaTime = DateTime.Now;
				player.ShowComponent("Death", true, "1");
				player.Notify("Information", "Du bist ins Koma gefallen!", NotificationType.INFO);
			}
		}

		[ServerEvent(ServerEventType.PLAYER_DEATH)]
		public static void OnPlayerDeath(RPPlayer player, IEntity? killer, uint weapon)
		{
			if (player.Interaction) player.StopInteraction(false);

			if (killer != null && killer.Type == BaseObjectType.Player)
			{
				var killerPlayer = (RPPlayer)killer;
				if(!killerPlayer.LoggedIn)
				{
					killerPlayer.Kick("Du wurdest gekicked!");
					player.Spawn(player.Position, 0);
					PlayerController.SetPlayerAlive(player, false);
					return;
				}

				player.LastAttackerId = killerPlayer.DbId;

				LogService.LogKill(player.DbId, killerPlayer.DbId, weapon);
			}

			if (killer != null && killer.Type == BaseObjectType.Vehicle)
			{
				var killerVehicle = (RPVehicle)killer;
				var killerPlayer = (RPPlayer)killerVehicle.NetworkOwner;
				if (!killerPlayer.LoggedIn)
				{
					killerPlayer.Kick("Du wurdest gekicked!");
					player.Spawn(player.Position, 0);
					PlayerController.SetPlayerAlive(player, false);
					return;
				}

				player.LastAttackerId = killerPlayer.DbId;

				LogService.LogKill(player.DbId, killerPlayer.DbId, weapon);
			}

			if (player.IsGangwar)
			{
				GangwarController.OnPlayerDeath(player);
			}

			if (player.IsInFFA)
			{
				if(killer != null && killer.Type == BaseObjectType.Player)
				{
					var killerPlayer = (RPPlayer)killer;

					killerPlayer.SetHealth(200);
					killerPlayer.SetArmor(100);
					killerPlayer.Notify("FFA", $"Du hast {player.Name} getötet!", NotificationType.SUCCESS);
					player.Notify("FFA", $"Du wurdest von {killerPlayer.Name} getötet!", NotificationType.WARN);
				}

				Task.Run(async () =>
				{
					await Task.Delay(2000);
					FFAController.SpawnPlayer(player);
				});
				return;
			}

			var injury = GetInjuryType(weapon);
			PlayerController.SetPlayerDead(player, injury);

			if(player.Dimension == 0 && player.Phone)
			{
				var dispatch = new Core.Models.Laptop.Dispatch(player.DbId, player.Name, "Notrufsignal", player.Position, "", DateTime.Now.ToString("HH:mm"), 3);
				LaptopModule.Dispatches.Add(dispatch);
				foreach (var target in RPPlayer.All.ToList())
				{
					if (!LaptopModule.HasAccessToDispatch(dispatch, target.TeamId)) continue;

					target.Notify("Dispatch", $"Es ist ein Dispatch von {player.Name} eingegangen!", NotificationType.INFO);
				}
			}
		}

		private static InjuryType GetInjuryType(uint weapon)
		{
			var weaponItemScript = InventoryController.GetWeaponItemScripts().FirstOrDefault(x => x.Hash == weapon);
			if(weaponItemScript != null) return weaponItemScript.Injury;

			if (SpecialInjuries.ContainsKey(weapon)) return SpecialInjuries[weapon];

			return InjuryType.PUNCH;
		}
	}
}