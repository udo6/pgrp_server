using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Core.Enums;
using AltV.Net.Enums;
using Core.Models.Gangwar;
using Newtonsoft.Json;
using Core;

namespace Game.Modules
{
	public static class GangwarModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach (var gangwar in GangwarService.GetAll())
				GangwarController.LoadGangwar(gangwar);

			Alt.OnClient<RPPlayer>("Server:Gangwar:Join", JoinGangwar);
			Alt.OnClient<RPPlayer>("Server:Gangwar:Interact", Interact);
			Alt.OnClient<RPPlayer, int>("Server:Gangwar:GetInformation", GetInformation);
			Alt.OnClient<RPPlayer, int>("Server:Gangwar:Attack", StartGangwar);
			Alt.OnClient<RPPlayer>("Server:Gangwar:Quit", QuitGangwar);
			Alt.OnClient<RPPlayer>("Server:Gangwar:SpawnVehicle", SpawnVehicle);
			Alt.OnClient<RPPlayer>("Server:Gangwar:OpenMenu", OpenMenu);
		}

		private static void JoinGangwar(RPPlayer player)
		{
			var gangwar = GangwarController.RunningGangwars.FirstOrDefault(x => x.AttackerId == player.TeamId || x.OwnerId == player.TeamId);
			if (gangwar == null) return;

			if(RPPlayer.All.Count(x => x.IsGangwar && x.Dimension == gangwar.DbId && x.TeamId == player.TeamId) >= 20)
			{
				player.Notify("Gangwar", "Es sind bereits 20 Personen im Gangwar!", NotificationType.ERROR);
				return;
			}

			GangwarController.JoinGangwar(player, gangwar);
		}

		private static void StartGangwar(RPPlayer player, int id)
		{
			if (player.TeamId <= 5) return;

			var shape = RPShape.Get(id, ColshapeType.GANGWAR_START);
			if (shape == null || player.Position.Distance(shape.Position) > shape.Size) return;

			var gangwar = GangwarService.Get(id);
			if (gangwar == null || gangwar.OwnerId == player.TeamId) return;

			var teamgws = GangwarService.GetFromTeam(player.TeamId);
			if(teamgws.Count >= 3)
			{
				player.Notify("Gangwar", "Deine Fraktion hat bereits 3 Gangwar Gebiete!", NotificationType.ERROR);
				return;
			}

			if(gangwar.OwnerId == 0)
			{
				TeamController.Broadcast(player.TeamId, $"Deine Fraktion hast das Gangwar Gebiet {gangwar.Name} für sich beansprucht!", NotificationType.SUCCESS);
				gangwar.OwnerId = player.TeamId;
				gangwar.LastAttack = DateTime.Now;
				GangwarService.Update(gangwar);
				return;
			}

			if (!Config.DevMode && gangwar.LastAttack.AddHours(36) > DateTime.Now)
			{
				player.Notify("Gangwar", "Das Gebiet wurde bereits in den letzten 36 Stunden attackiert!", NotificationType.ERROR);
				return;
			}

			if (GangwarController.RunningGangwars.FirstOrDefault(x => x.OwnerId == player.TeamId || x.AttackerId == player.TeamId) != null)
			{
				player.Notify("Gangwar", "Deine Fraktion befindet sich bereits in einem Gangwar!", NotificationType.ERROR);
				return;
			}

			var defenderOnline = RPPlayer.All.Count(x => x.TeamId == gangwar.OwnerId);

			if (!Config.DevMode && defenderOnline < 15)
			{
				player.Notify("Gangwar", "Es sind nicht genug Mitglieder der verteidigenden Fraktion online!", NotificationType.ERROR);
				return;
			}

			GangwarController.StartGangwar(player, gangwar);
		}

		private static void GetInformation(RPPlayer player, int id)
		{
			var gangwar = GangwarService.Get(id);
			if (gangwar == null) return;

			player.Notify("Information", $"Gebiet: {gangwar.Name}. Letzter angriff: {gangwar.LastAttack.Hour}:{gangwar.LastAttack.Minute} {gangwar.LastAttack.Day}.{gangwar.LastAttack.Month}.{gangwar.LastAttack.Year}", NotificationType.INFO);
		}

		private static void Interact(RPPlayer player)
		{
			if (player.TeamId <= 5) return;

			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.GANGWAR_START);
			if (shape == null) return;

			player.ShowNativeMenu(true, new("Gangwar", new()
			{
				new("Informationen abrufen", true, "Server:Gangwar:GetInformation", shape.ShapeId),
				new("Gebiet angreifen", true, "Server:Gangwar:Attack", shape.ShapeId)
			}));
		}

		private static void OpenMenu(RPPlayer player)
		{
			if(!player.IsGangwar) return;

			player.ShowNativeMenu(true, new("Gangwar", new()
			{
				new("Fahrzeug Spawnen", true, "Server:Gangwar:SpawnVehicle"),
				new("Gangwar Verlassen", true, "Server:Gangwar:Quit")
			}));
		}

		private static void SpawnVehicle(RPPlayer player)
		{
			var gangwar = GangwarController.RunningGangwars.FirstOrDefault(x => x.DbId == player.CurrentGangwarId);
			if (gangwar == null) return;

			var spawn = GangwarService.GetSpawns(gangwar.DbId, player.TeamId == gangwar.OwnerId).FirstOrDefault(x => RPVehicle.All.FirstOrDefault(e => e.Dimension == player.Dimension && PositionService.Get(x.PositionId)!.Position.Distance(e.Position) < 2) == null);
			if (spawn == null) return;

			var spawnPos = PositionService.Get(spawn.PositionId);
			if (spawnPos == null) return;

			var team = TeamService.Get(player.TeamId);
			if (team == null) return;

			var veh = (RPVehicle)Alt.CreateVehicle(VehicleModel.Revolter, spawnPos.Position, spawnPos.Rotation);
			veh.Dimension = gangwar.DbId;
			veh.SetLockState(false);
			veh.SetEngineState(true);
			veh.SetFuel(1000);
			veh.SetMaxFuel(1000);
			veh.Gangwar = true;
			veh.PrimaryColorRgb = new(team.ColorR, team.ColorG, team.ColorB, 255);
			veh.SecondaryColorRgb = new(team.ColorR, team.ColorG, team.ColorB, 255);
			veh.PearlColor = 0;
			veh.NumberplateText = team.ShortName;
		}

		private static void QuitGangwar(RPPlayer player)
		{
			if (!player.IsGangwar || !player.Alive) return;

			GangwarController.QuitGangwar(player);
		}

		[EveryTenSeconds]
		public static void FlagTick()
		{
			foreach(var gangwar in GangwarController.RunningGangwars.ToList())
			{
				var flags = RPShape.All.Where(x => x.ShapeType == ColshapeType.GANGWAR_FLAG && x.ShapeId == gangwar.DbId).ToList();
				foreach(var flag in flags)
				{
					var holder = RPPlayer.All.FirstOrDefault(x => x.Position.Distance(flag.Position) <= flag.Size);
					if (holder == null) continue;

					if(holder.TeamId == gangwar.OwnerId)
					{
						gangwar.OwnerPoints++;
					}
					else
					{
						gangwar.AttackerPoints++;
					}
				}

				GangwarController.UpdateHud(gangwar);
			}
		}

		[EveryMinute]
		public static void Tick()
		{
			foreach(var gangwar in GangwarController.RunningGangwars.ToList())
			{
				if (gangwar.Started.AddMinutes(GangwarController.GangwarDuration) > DateTime.Now) continue;

				GangwarController.StopGangwar(gangwar);
			}
		}
	}
}