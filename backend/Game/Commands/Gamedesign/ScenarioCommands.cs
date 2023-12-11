using AltV.Net;
using AltV.Net.Data;
using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Lootdrop;
using Database.Services;

namespace Game.Commands.Gamedesign
{
	public static class ScenarioCommands
	{
		[Command("createdrop")]
		public static void CreateDrop(RPPlayer player, string name)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var offset = new Position(15, 9, 46);

			var pos = new PositionModel(player.Position - offset);
			PositionService.Add(pos);

			var drop = new LootdropModel(name.Replace('_', ' '), pos.Id, 0, 0, 0);
			LootdropService.Add(drop);
			var obj = Alt.CreateObject(249853152, pos.Position, pos.Rotation);
			obj.Frozen = true;
			player.Notify("Gamedesign", $"ID: {drop.Id}", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("setdropbox1")]
		public static void SetDropBox1(RPPlayer player, int dropId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var drop = LootdropService.Get(dropId);
			if (drop == null) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			drop.Box1PositionId = pos.Id;
			LootdropService.Update(drop);
			var obj = Alt.CreateObject(1776043012, pos.Position, pos.Rotation);
			obj.Frozen = true;
		}

		[Command("setdropbox2")]
		public static void SetDropBox2(RPPlayer player, int dropId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var drop = LootdropService.Get(dropId);
			if (drop == null) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			drop.Box2PositionId = pos.Id;
			LootdropService.Update(drop);
			var obj = Alt.CreateObject(1776043012, pos.Position, pos.Rotation);
			obj.Frozen = true;
		}

		[Command("setdropbox3")]
		public static void SetDropBox3(RPPlayer player, int dropId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var drop = LootdropService.Get(dropId);
			if (drop == null) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			drop.Box3PositionId = pos.Id;
			LootdropService.Update(drop);
			var obj = Alt.CreateObject(1776043012, pos.Position, pos.Rotation);
			obj.Frozen = true;
		}
	}
}