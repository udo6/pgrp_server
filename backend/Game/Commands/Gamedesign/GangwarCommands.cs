﻿using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Gangwar;
using Database.Services;
using Game.Controllers;
using Game.Streamer;

namespace Game.Commands.Gamedesign
{
	public static class GangwarCommands
	{
		[Command("creategw")]
		public static void CreateGangwar(RPPlayer player, string name)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var gw = new GangwarModel(name.Replace('_', ' '), pos.Id, 0, DateTime.Now.AddHours(-72), 0, 0);
			GangwarService.Add(gw);
			MarkerStreamer.AddMarker(new(
				1,
				new(pos.X, pos.Y, pos.Z - 30),
				new(400, 400, 400),
				new(0, 155, 255, 255),
				0,
				gw.Id));
		}

		[Command("addgwspawn1")]
		public static void AddGangwarSpawn1(RPPlayer player, int gangwarId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var gw = GangwarService.Get(gangwarId);
			if (gw == null) return;

			gw.DefenderSpawnPositionId = pos.Id;
			GangwarService.Update(gw);
		}

		[Command("addgwspawn2")]
		public static void AddGangwarSpawn2(RPPlayer player, int gangwarId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var gw = GangwarService.Get(gangwarId);
			if (gw == null) return;

			gw.AttackerSpawnPositionId = pos.Id;
			GangwarService.Update(gw);
		}

		[Command("addgwvehiclespawn1")]
		public static void AddGangwarVehicleSpawn1(RPPlayer player, int gangwarId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			if (!player.IsInVehicle) return;

			var pos = new PositionModel(player.Vehicle.Position, player.Vehicle.Rotation);
			PositionService.Add(pos);

			var spawn = new GangwarSpawnModel(gangwarId, pos.Id, true);
			GangwarService.AddSpawn(spawn);
		}

		[Command("addgwvehiclespawn2")]
		public static void AddGangwarVehicleSpawn2(RPPlayer player, int gangwarId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			if (!player.IsInVehicle) return;

			var pos = new PositionModel(player.Vehicle.Position, player.Vehicle.Rotation);
			PositionService.Add(pos);

			var spawn = new GangwarSpawnModel(gangwarId, pos.Id, false);
			GangwarService.AddSpawn(spawn);
		}

		[Command("addgwflag")]
		public static void AddGangwarFlag(RPPlayer player, int gangwarId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var flag = new GangwarFlagModel(gangwarId, pos.Id);
			GangwarService.AddFlag(flag);
		}

		[Command("loadgw")]
		public static void LoadGangwar(RPPlayer player, int gangwarId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var gw = GangwarService.Get(gangwarId);
			if (gw == null) return;

			GangwarController.LoadGangwar(gw);
		}
	}
}