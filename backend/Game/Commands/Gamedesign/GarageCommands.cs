using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Garage;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class GarageCommands
	{
		[Command("creategarage")]
		public static void CreateGarage(RPPlayer player, string name, int type, int owner, int ownerType)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var garage = new GarageModel(name.Replace('_', ' '), pos.Id, 0, (GarageType)type, owner, (OwnerType)ownerType);
			GarageService.Add(garage);

			player.Notify("Gamedesign", $"Du hast eine Garage erstellt! ID: {garage.Id}", NotificationType.INFO);
		}

		[Command("addgaragespawn")]
		public static void AddGarageSpawn(RPPlayer player, int garageId)
		{
			if ((!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) || !player.IsInVehicle) return;

			var pos = new PositionModel(player.Vehicle.Position, player.Vehicle.Rotation);
			PositionService.Add(pos);

			var spawn = new GarageSpawnModel(garageId, pos.Id);
			GarageService.AddSpawn(spawn);

			player.Notify("Gamedesign", $"Du hast eine Spawn Punkt hinzugefügt!", NotificationType.INFO);
		}

		[Command("addgarageped")]
		public static void AddGaragePed(RPPlayer player, int garageId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var garage = GarageService.Get(garageId);
            if (garage == null) return;

            var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			garage.PedPositionId = pos.Id;
			GarageService.Update(garage);

			player.Notify("Gamedesign", $"Du hast eine Ped hinzugefügt!", NotificationType.INFO);
		}

		[Command("loadgarage")]
		public static void LoadGarage(RPPlayer player, int garageId)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var garage = GarageService.Get(garageId);
			if (garage == null) return;

			GarageController.LoadGarage(garage);
		}
	}
}