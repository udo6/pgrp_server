using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Door;
using Database.Services;

namespace Game.Controllers
{
	public static class DoorController
	{
		public static void LoadDoor(DoorModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), model.Radius, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.DOOR;
			shape.Size = model.Radius;
		}

		public static bool HasDoorAccess(int doorId, int playerId, int teamId)
		{
			var accessList = DoorService.GetAccesses(doorId);
			return accessList.Any(x =>
			(x.OwnerType == Core.Enums.OwnerType.PLAYER && x.OwnerId == playerId) ||
			((x.OwnerType == Core.Enums.OwnerType.TEAM && x.OwnerId == teamId)));
		}
	}
}
