using Core;
using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Barber;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class BarberCommands
	{
		[Command("createbarber")]
		public static void CreateBarber(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var barber = new BarberModel(pos.Id);
			BarberService.Add(barber);
			BarberController.LoadBarber(barber);
		}

		[Command("addbarberstyle")]
		public static void AddBarberStyle(RPPlayer player, int barberId, int style, string label, int gender)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var model = new BarberStyleModel(barberId, label, style, 450, gender);
			BarberService.AddStyle(model);
		}
	}
}