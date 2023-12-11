using Database.Models.Door;
using Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Controllers
{
	public static class DoorController
	{
		public static void LoadDoor(DoorModel model)
		{

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
