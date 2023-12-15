using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Core.Models.Gangwar;
using Database.Models.Gangwar;
using Database.Services;
using Newtonsoft.Json;
using AltV.Net.Shared.Enums;
using Game.Streamer;

namespace Game.Controllers
{
    public static class GangwarController
	{
		public static readonly int GangwarDuration = 1;
		public static readonly List<RunningGangwar> RunningGangwars = new();

		public static List<uint> Weapons = new()
		{
			2210333304, // CARBINERIFLE
			2937143193, // ADVACNEDRIFLE
			3231910285, // SPECIALCARBINE
			2132975508, // BULLPUPRIFLE
		};

		public static void LoadGangwar(GangwarModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 1.5f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.GANGWAR_START;
			shape.Size = 1.5f;

			var ped = Alt.CreatePed(AltV.Net.Enums.PedModel.Michael, pos.Position, pos.Rotation);
			ped.Frozen = true;

			var team = TeamService.Get(model.OwnerId);

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.Name = "Gangwar Gebiet";
			blip.Sprite = 543;
			blip.ShortRange = true;
			blip.Color = (byte)(team == null ? 1 : team.BlipColor);
		}

		public static void StartGangwar(RPPlayer player, GangwarModel gangwar)
		{
			var gwPos = PositionService.Get(gangwar.PositionId);
			if (gwPos == null) return;

			TeamController.Broadcast(player.TeamId, $"Ihr greift das Gangwar Gebiet {gangwar.Name} an!", NotificationType.INFO);
			TeamController.Broadcast(gangwar.OwnerId, $"Euer Gangwar Gebiet {gangwar.Name} wird angegriffen!", NotificationType.INFO);

			gangwar.LastAttack = DateTime.Now;
			GangwarService.Update(gangwar);

			var attacker = TeamService.Get(player.TeamId);
			var owner = TeamService.Get(gangwar.OwnerId);
			if (attacker == null || owner == null) return;

			var attackerSpawn = PositionService.Get(gangwar.AttackerSpawnPositionId);
			var defenderSpawn = PositionService.Get(gangwar.DefenderSpawnPositionId);
			if (attackerSpawn == null || defenderSpawn == null) return;

			var attackerShape = (RPShape)Alt.CreateColShapeCylinder(attackerSpawn.Position.Down(), 2f, 2f);
			attackerShape.Id = gangwar.Id;
			attackerShape.ShapeType = ColshapeType.GANGWAR_SPAWN;
			attackerShape.Dimension = gangwar.Id;
			attackerShape.Size = 2f;

			var defenderShape = (RPShape)Alt.CreateColShapeCylinder(defenderSpawn.Position.Down(), 2f, 2f);
			defenderShape.Id = gangwar.Id;
			defenderShape.ShapeType = ColshapeType.GANGWAR_SPAWN;
			defenderShape.Dimension = gangwar.Id;
			defenderShape.Size = 2f;

			// markers
			var mainMarker = MarkerStreamer.AddMarker(new(
				1,
				new(gwPos.Position.X, gwPos.Position.Y, gwPos.Position.Z - 30),
				new(300, 300, 300),
				new(0, 155, 255, 255),
				0,
				gangwar.Id));

			var attackerSpawnMarker = MarkerStreamer.AddMarker(new(
				21,
				attackerSpawn.Position,
				new(1, 1, 1),
				new(0, 155, 255, 255),
				0,
				false,
				true,
				false,
				gangwar.Id));

			var defenderSpawnMarker = MarkerStreamer.AddMarker(new(
				21,
				defenderSpawn.Position,
				new(1, 1, 1),
				new(0, 155, 255, 255),
				0,
				false,
				true,
				false,
				gangwar.Id));

			var gw = new RunningGangwar(gangwar.Id, gangwar.Name, owner.Id, owner.Name, 0, attacker.Id, attacker.Name, 0, mainMarker, attackerSpawnMarker, defenderSpawnMarker);
			RunningGangwars.Add(gw);

			var flags = GangwarService.GetFlags(gangwar.Id);

			foreach (var flag in flags)
			{
				var pos = PositionService.Get(flag.PositionId);
				if (pos == null) continue;

				var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
				shape.Id = gangwar.Id;
				shape.ShapeType = ColshapeType.GANGWAR_FLAG;
				shape.Dimension = gangwar.Id;
				shape.Size = 2f;
				shape.Dimension = gangwar.Id;

				var marker = MarkerStreamer.AddMarker(new(
				4,
				pos.Position,
				new(1, 1, 1),
				new(0, 155, 255, 255),
				0,
				false,
				true,
				false,
				gangwar.Id));

				gw.Markers.Add(marker);
			}

			JoinGangwar(player, gw);
		}

		public static void StopGangwar(RunningGangwar gw)
		{
			var gangwar = GangwarService.Get(gw.DbId);
			if (gangwar == null) return;

			var owner = TeamService.Get(gw.OwnerId);
			var attacker = TeamService.Get(gw.AttackerId);
			if (owner == null || attacker == null) return;

			foreach (var player in RPPlayer.All.ToList())
			{
				if (player == null || !player.Exists || player.CurrentGangwarId != gw.DbId) continue;
				RespawnPlayer(player);
				QuitGangwar(player);
				player.GangwarWeapon = -1;
			}

			if (gw.AttackerPoints > gw.OwnerPoints)
			{
				TeamController.Broadcast(attacker.Id, "Ihr habt das Gebiet erfolgreich eingenommen!", NotificationType.INFO);
				TeamController.Broadcast(owner.Id, "Ihr konntet das Gebiet nicht verteidigen!", NotificationType.INFO);

				gangwar.OwnerId = gw.AttackerId;
				GangwarService.Update(gangwar);
			}
			else
			{
				TeamController.Broadcast(owner.Id, "Ihr habt das Gebiet erfolgreich verteidigen!", NotificationType.INFO);
				TeamController.Broadcast(attacker.Id, "Ihr konntet das Gebiet nicht einnehmen!", NotificationType.INFO);
			}

			foreach (var shape in RPShape.All.ToList())
			{
				if (shape == null || !shape.Exists || shape.Id != gw.DbId || (shape.ShapeType != ColshapeType.GANGWAR_FLAG && shape.ShapeType != ColshapeType.GANGWAR_SPAWN)) continue;

				RPShape.All.Remove(shape);
				shape.Destroy();
			}

			foreach (var veh in RPVehicle.All.ToList())
			{
				if (veh == null || !veh.Exists || veh.Dimension != gw.DbId || !veh.Gangwar) continue;

				veh.Delete();
			}

			MarkerStreamer.RemoveMarkers(gw.Markers);
			RunningGangwars.Remove(gw);
		}

		public static void RespawnPlayer(RPPlayer player)
		{
			var gangwar = GangwarService.Get(player.CurrentGangwarId);
			if (gangwar == null) return;

			var pos = PositionService.Get(gangwar.OwnerId == player.TeamId ? gangwar.DefenderSpawnPositionId : gangwar.AttackerSpawnPositionId);
			if (pos == null) return;

			player.SetPosition(pos.Position);
			PlayerController.SetPlayerAlive(player, false);
			InventoryController.AddItem(player.InventoryId, 6, 10);
			InventoryController.AddItem(player.InventoryId, 7, 10);
			InventoryController.AddItem(player.InventoryId, 26, 5);
		}

		public static void JoinGangwar(RPPlayer player, RunningGangwar gangwar)
		{
			var gw = GangwarService.Get(gangwar.DbId);
			if (gw == null) return;

			var spawn = PositionService.Get(gangwar.OwnerId == player.TeamId ? gw.DefenderSpawnPositionId : gw.AttackerSpawnPositionId);
			if (spawn == null) return;

			var team = TeamService.Get(player.TeamId);
			if (team == null) return;

			var respawn = PositionService.Get(team.PositionId);
			if (respawn == null) return;

			player.EmitBrowser("Hud:ShowGangwar", true, JsonConvert.SerializeObject(GetData(gangwar, true)));
			player.ShowComponent("Team", false);
			player.CurrentGangwarId = gw.Id;
			player.InInterior = true;
			player.OutsideInteriorPosition = respawn.Position;
			player.SetPosition(spawn.Position);
			player.SetDimension(gw.Id);

			if(player.GangwarWeapon == -1)
			{
				player.GangwarWeapon = new Random().Next(0, Weapons.Count);
			}

			player.RemoveAllWeapons(true);
			player.Weapons.Clear();
			player.AddWeapon(Weapons[player.GangwarWeapon], 9999, true);
			player.AddWeapon(3219281620, 9999, false);
			player.AddWeapon(team.MeeleWeaponHash, 9999, false);
			InventoryController.AddItem(player.InventoryId, 6, 10);
			InventoryController.AddItem(player.InventoryId, 7, 10);
			InventoryController.AddItem(player.InventoryId, 26, 5);
		}

		public static void QuitGangwar(RPPlayer player)
		{
			player.CurrentGangwarId = 0;
			player.InInterior = false;
			player.SetPosition(player.OutsideInteriorPosition);
			player.SetDimension(0);
			PlayerController.ApplyPlayerLoadout(player);
			player.EmitBrowser("Hud:ShowGangwar", false, "");
		}

		public static object? GetData(RunningGangwar gangwar, bool useTime)
		{
			var attacker = TeamService.Get(gangwar.AttackerId);
			var owner = TeamService.Get(gangwar.OwnerId);
			if (attacker == null || owner == null) return null;

			var time = (int)(useTime ? Math.Floor((gangwar.Started.AddMinutes(GangwarDuration) - DateTime.Now).TotalSeconds) : 0);

			return new
			{
				Attacker = new
				{
					Name = attacker.Name,
					Label = attacker.ShortName,
					Points = gangwar.AttackerPoints
				},
				Defender = new
				{
					Name = owner.Name,
					Label = owner.ShortName,
					Points = gangwar.OwnerPoints
				},
				Time = time
			};
		}

		public static void OnPlayerDeath(RPPlayer player)
		{
			var gw = RunningGangwars.FirstOrDefault(x => x.OwnerId == player.TeamId || x.AttackerId == player.TeamId);
			if (gw == null) return;

			if(player.TeamId == gw.OwnerId) gw.AttackerPoints += 3;
			else gw.OwnerPoints += 3;

			UpdateHud(gw);
		}

		public static void UpdateHud(RunningGangwar gangwar)
		{
			var data = GetData(gangwar, false);
			var json = JsonConvert.SerializeObject(data);

			foreach (var player in RPPlayer.All.ToList())
			{
				if (player == null || !player.Exists || player.CurrentGangwarId != gangwar.DbId) continue;
				player.EmitBrowser("Hud:UpdateGangwar", json);
			}
		}
	}
}