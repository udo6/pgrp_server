using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Door;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class DoorlockCommands
	{
		[Command("createdoorlock")]
		public static void CreateDoorlock(RPPlayer player, int radius)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var model = new DoorModel(pos.Id, true, radius);
			DoorService.Add(model);
			DoorController.LoadDoor(model);
			player.Notify("Information", $"ID: {model.Id}", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("adddoorentity")]
		public static void AddDoorEntity(RPPlayer player, int doorId, int hash)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var model = new DoorEntityModel(doorId, (uint)hash, pos.Id);
			DoorService.AddEntity(model);
			player.Notify("Information", "Entity added", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("adddooraccess")]
		public static void AddDoorAccess(RPPlayer player, int doorId, int ownerId, int ownerType)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var model = new DoorAccessModel(doorId, ownerId, (OwnerType)ownerType);
			DoorService.AddAccess(model);
			player.Notify("Information", "Access added", Core.Enums.NotificationType.SUCCESS);
		}
	}
}