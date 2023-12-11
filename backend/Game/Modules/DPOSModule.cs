using AltV.Net;
using Core.Attribute;
using Core.Entities;
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

			Alt.OnClient<RPPlayer>("Server:Impound:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:Impound:TakeVehicle", TakeVehicle);
		}

		private static void Open(RPPlayer player)
		{
			var shape = RPShape.All.FirstOrDefault(x => x.ShapeType == Core.Enums.ColshapeType.IMPOUND && x.Position.Distance(player.Position) <= x.Size);
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

			var spawn = ImpoundController.GetFreeSpawn(shape.Id);
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