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

		[Command("createlab")]
		public static void CreateLaboratory(RPPlayer player, int teamId, int type)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var team = TeamService.Get(teamId);
			if (team == null) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var fuel = new InventoryModel(8, 40, InventoryType.LAB_FUEL);
			var rob = new InventoryModel(8, 40, InventoryType.LAB_ROB);
			InventoryService.Add(fuel, rob);

			var lab = new LaboratoryModel(teamId, pos.Id, fuel.Id, false, rob.Id, (LaboratoryType)type);
			TeamService.AddLaboratory(lab);

			var jumppoint = new JumppointModel($"{team.ShortName} - Labor", pos.Id, 0, LaboratoryController.LabInsidePositionIds[type], lab.Id, teamId, 0, OwnerType.TEAM, true, DateTime.Now.AddHours(-92), JumppointType.LABORATORY);
			JumppointService.Add(jumppoint);

			LaboratoryController.LoadLaboratory(lab);
			JumppointController.LoadJumppoint(jumppoint);
		}
	}
}