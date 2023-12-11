using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Tuner;
using Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Controllers
{
	public static class TunerController
	{
		public static void LoadTuner(TunerModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 5f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.TUNER;
			shape.Size = 1.5f;
		}
	}
}
