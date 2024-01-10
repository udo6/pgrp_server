using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Hospital;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class HospitalCommands
	{
		[Command("createhospital")]
		public static void CreateHospital(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var model = new HospitalModel(pos.Id, 0);
			HospitalService.Add(model);
			HospitalController.LoadHospital(model);
			player.Notify("Gamedesign", "Du hast ein Krankenhaus erstellt!", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("addhospitalbed")]
		public static void AddHospitalBed(RPPlayer player, int hospitalId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position);
			PositionService.Add(pos);

			var model = new HospitalBedModel(hospitalId, pos.Id);
			HospitalService.AddBed(model);
			player.Notify("Gamedesign", "Du hast ein Krankenhaus Bett erstellt!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
