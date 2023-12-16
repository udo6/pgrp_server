using AltV.Net;
using Core.Entities;
using Database.Models.Workstation;
using Database.Services;
using Core.Enums;
using Core.Extensions;
using AltV.Net.Elements.Pools;

namespace Game.Controllers
{
	public static class WorkstationController
	{
		public static void LoadWorkstation(WorkstationModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 1f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.WORKSTATION;
			shape.Size = 1f;

			var ped = Alt.CreatePed(3446096293, pos.Position, pos.Rotation);
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;

			if(model.Type == WorkstationType.WEAPON_FACTORY)
			{
				var random = new Random();
				var bps = WorkstationService.GetBlueprints(model.Id, false);
				foreach (var bp in bps) bp.Active = false;

				if (bps.Count <= 0) return;
				for(var i = 0; i < model.MaxActiveItems; i++)
				{
					var bpss = bps.Where(x => !x.Active).ToList();
					bpss[random.Next(0, bpss.Count)].Active = true;
				}
			}
		}

		public static bool HasAccess(WorkstationModel workstation, RPPlayer player)
		{
			return workstation.Type == WorkstationType.PUBLIC ||
				(workstation.Type == WorkstationType.TEAM_ONLY && player.TeamId > 5) ||
				(workstation.Type == WorkstationType.WEAPON_FACTORY && player.TeamId > 5);
		}
	}
}