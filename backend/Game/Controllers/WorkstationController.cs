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
		}
	}
}