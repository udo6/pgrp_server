using Core;
using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class BlipCommands
	{
		[Command("createblip")]
		public static void CreateBlip(RPPlayer player, string name, int sprite, int color, int shortRange)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var blip = new BlipModel(name.Replace('_', ' '), pos.Id, sprite, color, Convert.ToBoolean(shortRange));
			BlipService.Add(blip);
			BlipController.LoadBlip(blip);
		}
	}
}