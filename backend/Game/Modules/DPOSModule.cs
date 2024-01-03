using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class DPOSModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in ImpoundService.GetAll())
				ImpoundController.LoadImpound(model);

			Alt.OnClient<RPPlayer, string>("Server:Impound:SetPlate", SetVehiclePlate);
			Alt.OnClient<RPPlayer>("Server:Impound:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:Impound:TakeVehicle", TakeVehicle);
		}

		private static void SetVehiclePlate(RPPlayer player, string plate)
		{
			if (player.TeamId != 4 || !player.TeamDuty || !player.IsInVehicle) return;

			if (plate.Length > 8)
			{
				player.Notify("Information", "Das Kennzeichen darf nicht länger als 8 Zeichen lang sein!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var vehicle = (RPVehicle)player.Vehicle;
			if (vehicle.DbId < 1) return;

			var dbVehicle = VehicleService.Get(vehicle.DbId);
			if (dbVehicle == null) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if (account.Money < 5000)
			{
				player.Notify("Information", "Du benötigst $5000 um das Kennzeichen eunzuba", Core.Enums.NotificationType.ERROR);
				return;
			}

			PlayerController.RemoveMoney(player, 5000);
			vehicle.NumberplateText = plate;
			dbVehicle.Plate = plate;
			VehicleService.UpdateVehicle(dbVehicle);
			player.Notify("Information", "Du hast ein Kennzeichen verbaut!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void Open(RPPlayer player)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.IMPOUND);
			if (shape == null) return;

			var vehicles = VehicleService.GetPlayerImpoundedVehicles(player.DbId);
			var nativeItems = new List<NativeMenuItem>();

			foreach(var veh in vehicles)
			{
				var vehBase = VehicleService.GetBase(veh.BaseId);
				if (vehBase == null) continue;

				nativeItems.Add(new(vehBase.Name, true, "Server:Impound:TakeVehicle", veh.Id));
			}

			player.ShowNativeMenu(true, new("Abschlepphof", nativeItems));
		}

		private static void TakeVehicle(RPPlayer player, int vehId)
		{
			var shape = RPShape.All.FirstOrDefault(x => x.ShapeType == Core.Enums.ColshapeType.IMPOUND && x.Position.Distance(player.Position) <= x.Size);
			if (shape == null) return;

			var spawn = ImpoundController.GetFreeSpawn(shape.ShapeId);
			if (spawn == null)
			{
				player.Notify("Information", "Es ist kein Ausparkpunkt frei!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(account.Money < 2500)
			{
				player.Notify("Information", "Du benötigst $2500!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var veh = VehicleService.Get(vehId);
			if (veh == null) return;

			account.Money -= 2500;
			AccountService.Update(account);

			VehicleController.LoadVehicle(veh, spawn);
			player.Notify("Information", "Du hast dein Fahrzeug für $2500 freigekauft!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}