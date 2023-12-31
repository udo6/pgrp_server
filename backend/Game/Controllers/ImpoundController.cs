using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Pools;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models;
using Database.Models.DPOS;
using Database.Services;

namespace Game.Controllers
{
	public static class ImpoundController
	{
		public static void LoadImpound(ImpoundModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 1.5f, 2f);
			shape.ShapeId = model.Id;
			shape.ShapeType = ColshapeType.IMPOUND;
			shape.Size = 1.5f;

			var ped = Alt.CreatePed(3446096293, pos.Position, pos.Rotation);
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.Name = "Abschlepphof";
			blip.Sprite = 446;
			blip.Color = 4;
			blip.ShortRange = true;
		}

		public static PositionModel? GetFreeSpawn(int impoundId)
		{
			var spawns = ImpoundService.GetSpawns(impoundId);

			foreach(var spawn in spawns)
			{
				var pos = PositionService.Get(spawn.PositionId);
				if (pos == null || RPVehicle.All.Any(x => x.Position.Distance(pos.Position) < 3f)) continue;

				return pos;
			}

			return null;
		}
	}
}