using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Jumpoint;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class JumppointCommands
	{
		[Command("createjumppoint")]
		public static void CreateJumppoint(RPPlayer player, string name, int dimension, int ownerId, int ownerType, int locked, int type)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var jumppoint = new JumppointModel(name.Replace('_', ' '), pos.Id, dimension, 0, 0, ownerId, 0, (OwnerType)ownerType, Convert.ToBoolean(locked), DateTime.Now.AddMinutes(-50), (JumppointType)type);
			JumppointService.Add(jumppoint);
		}

		[Command("setjumppointpos2")]
		public static void SetJumppointPos2(RPPlayer player, int jumppointId, int dimension)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var jumppoint = JumppointService.Get(jumppointId);
			if (jumppoint == null) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			jumppoint.InsidePositionId = pos.Id;
			jumppoint.InsideDimension = dimension;
			JumppointService.Update(jumppoint);
			JumppointController.LoadJumppoint(jumppoint);
		}
	}
}