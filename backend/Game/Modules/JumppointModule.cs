using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Core.Enums;

namespace Game.Modules
{
	public static class JumppointModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in JumppointService.GetAll())
				JumppointController.LoadJumppoint(model);

			Alt.OnClient<RPPlayer>("Server:JumpPoint:Lock", Lock);
			Alt.OnClient<RPPlayer>("Server:JumpPoint:Enter", Enter);
		}

		private static void Lock(RPPlayer player)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.JUMP_POINT);
			if (shape == null) return;

			var jumppoint = JumppointService.Get(shape.Id);
			if (jumppoint == null) return;

			if((jumppoint.OwnerType == OwnerType.PLAYER && jumppoint.OwnerId != player.DbId && jumppoint.KeyHolderId != player.DbId) || (jumppoint.OwnerType == OwnerType.TEAM && jumppoint.OwnerId != player.TeamId))
			{
				return;
			}

			if(jumppoint.LastCrack.AddMinutes(30) > DateTime.Now)
			{
				player.Notify("Jumppoint", "Das Schloss ist noch kaputt!", NotificationType.ERROR);
				return;
			}

			jumppoint.Locked = !jumppoint.Locked;
			JumppointService.Update(jumppoint);
			player.Notify("Jumppoint", jumppoint.Locked ? "Abgeschlossen!" : "Aufgeschlossen!", jumppoint.Locked ? NotificationType.ERROR : NotificationType.SUCCESS);
		}

		private static void Enter(RPPlayer player)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.JUMP_POINT);
			if (shape == null) return;

			var jumppoint = JumppointService.Get(shape.Id);
			if (jumppoint == null || jumppoint.Locked) return;

			var outside = PositionService.Get(jumppoint.OutsidePositionId);
			var inside = PositionService.Get(jumppoint.InsidePositionId);
			if (outside == null || inside == null) return;

			var targetPos = shape.JumppointEnterType ? inside : outside;
			var targetDim = shape.JumppointEnterType ? jumppoint.InsideDimension : jumppoint.OutsideDimension;

			if (jumppoint.InsideDimension > 0 && jumppoint.OutsideDimension > 0)
			{
				player.InInterior = shape.JumppointEnterType;
				player.OutsideInteriorPosition = outside.Position;
			}

			player.SetPosition(targetPos.Position);
			player.Rotation = targetPos.Rotation;
			player.SetDimension(targetDim);
		}
	}
}