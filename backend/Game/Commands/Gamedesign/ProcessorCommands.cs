using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Processor;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class ProcessorCommands
	{
		[Command("createprocessor")]
		public static void CreateProcessor(RPPlayer player, int inputItem, int inputStep, int outputItem, int outputStep, int time)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var model = new ProcessorModel(pos.Id, inputItem, inputStep, outputItem, outputStep, time);
			ProcessorService.Add(model);
			ProcessorController.LoadProcessor(model);
			player.Notify("Gamedesign", "Du hast einen Verarbeiter erstellt!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
