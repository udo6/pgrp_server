using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Jumpoint;
using Database.Services;

namespace Game.Controllers
{
    public static class JumppointController
	{
		public static void LoadJumppoint(JumppointModel model)
		{
			var outsidePos = PositionService.Get(model.OutsidePositionId);
			if (outsidePos == null) return;

			var outside = (RPShape)Alt.CreateColShapeCylinder(outsidePos.Position.Down(), 1f, 2f);
			outside.ShapeId = model.Id;
			outside.ShapeType = ColshapeType.JUMP_POINT;
			outside.Size = 1.5f;
			outside.Dimension = model.OutsideDimension;
			outside.JumppointEnterType = true;

			var insidePos = PositionService.Get(model.InsidePositionId);
			if (insidePos == null) return;

			var inside = (RPShape)Alt.CreateColShapeCylinder(insidePos.Position.Down(), 1f, 2f);
			inside.ShapeId = model.Id;
			inside.ShapeType = ColshapeType.JUMP_POINT;
			inside.Size = 1.5f;
			inside.Dimension = model.InsideDimension;
			inside.JumppointEnterType = false;
		}
	}
}