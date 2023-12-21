using AltV.Net.Elements.Entities;
using AltV.Net;
using Core.Entities;
using Core.Enums;
using Database.Models.Hospital;
using Database.Services;
using Core.Extensions;

namespace Game.Controllers
{
	public static class HospitalController
	{
		public static void LoadHospital(HospitalModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(5f), 4f, 10f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.HOSPITAL;
			shape.Size = 4f;

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.Name = $"Krankenhaus";
			blip.Sprite = 61;
			blip.Color = 1;
			blip.ShortRange = true;
		}

		public static void TakeInPlayer(RPPlayer player, HospitalBedModel bed)
		{
			var pos = PositionService.Get(bed.PositionId);
			if (pos == null) return;

			player.SetPosition(pos.Position);
			player.Rotation = pos.Rotation;
			player.IsInHospital = true;
		}
	}
}