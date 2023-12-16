using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Workstation;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class WorkstationCommands
	{
		[Command("createworkstation")]
		public static void CreateWorkstation(RPPlayer player, int type, int maxActiveItems)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var model = new WorkstationModel(pos.Id, (WorkstationType)type, maxActiveItems);
			WorkstationService.Add(model);
			WorkstationController.LoadWorkstation(model);
			player.Notify("Gamedesign", "Du hast eine Workstation erstellt!", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("addworkstationitem")]
		public static void AddWorkstationItem(RPPlayer player, int stationId, int itemId, int itemAmount, int price, int neededItem, int neededItemAmount, int duration, int max)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var model = new WorkstationBlueprintModel(stationId, itemId, itemAmount, price, neededItem, neededItemAmount, duration, max, false);
			WorkstationService.AddBlueprint(model);
			player.Notify("Gamedesign", "Du hast ein Item hinzugefügt erstellt!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
