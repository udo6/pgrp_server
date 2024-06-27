using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Inventory;
using Database.Models.Jumpoint;
using Database.Models.Team;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class TeamCommands
	{
		[Command("createteam")]
		public static void CreateTeam(RPPlayer player, string name, string shortName, int r, int g, int b, int blipColor, int type, string meeleName, int meeleHash)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var team = new TeamModel(
				name.Replace('_', ' '),
				shortName.Replace('_', ' '),
				pos.Id,
				(byte)r,
				(byte)g,
				(byte)b,
				(byte)blipColor,
				(TeamType)type,
				0,
				meeleName.Replace('_', ' '),
				(uint)meeleHash,
				0);
			TeamService.Add(team);

			TeamController.LoadTeam(team);
		}
	}
}