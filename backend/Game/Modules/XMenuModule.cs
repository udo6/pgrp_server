using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.Input;
using Database.Models.Garage;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules
{
    public static class XMenuModule
	{
		[Initialize]
		public static void Initialize()
		{
			// VEHICLE
			Alt.OnClient<RPPlayer, int>("Server:XMenu:GetVehicleId", GetVehicleId);
			Alt.OnClient<RPPlayer>("Server:XMenu:ToggleEngine", ToggleEngine);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:LockVehicle", Lock);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:LockTrunk", LockTrunk);
			Alt.OnClient<RPPlayer>("Server:XMenu:Eject", Eject);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:Repair", Repair);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:OpenFuel", OpenFuel);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:Park", Park);

			// PLAYER
			Alt.OnClient<RPPlayer, int>("Server:XMenu:GetPlayerId", GetPlayerId);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:GiveKey", GiveKey);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:Revive", RevivePlayer);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:Search", SearchPlayer);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:Cuff", CuffPlayer);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:Rope", RopePlayer);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:Grab", GrabIntoVehicle);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:GiveLicense", GiveLicense);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:TakeLicense", TakeLicense);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:GiveId", GiveID);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:TakeId", TakeID);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:GiveMoney", GiveMoney);
			Alt.OnClient<RPPlayer, int>("Server:XMenu:Stabilize", StabilizePlayer);
		}

		// VEHICLE
		private static void GetVehicleId(RPPlayer player, int vehicleId)
		{
			if (!player.LoggedIn) return;

			var veh = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId);
			if (veh == null) return;

			player.Notify("Information", $"Fahrzeug ID: {veh.DbId}", NotificationType.INFO);
		}

		private static void ToggleEngine(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.Vehicle.Driver != player || ((RPVehicle)player.Vehicle).Fuel <= 0) return;

			var veh = (RPVehicle)player.Vehicle;

			if (!VehicleController.IsVehicleOwner(veh, player)) return;

			veh.SetEngineState(!veh.Engine);
			player.Notify("Fahrzeug", veh.Engine ? "Eingeschaltet." : "Ausgeschaltet.", veh.Engine ? NotificationType.SUCCESS : NotificationType.ERROR);
		}

		private static void Lock(RPPlayer player, int vehicleId)
		{
			if (!player.LoggedIn) return;

			var veh = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId);
			if(veh == null || !VehicleController.IsVehicleOwner(veh, player)) return;

			veh.SetLockState(!veh.Locked);
			player.Notify("Fahrzeug", veh.Locked ? "Abgeschlossen." : "Aufgeschlossen.", veh.Locked ? NotificationType.ERROR : NotificationType.SUCCESS);
		}

		private static void LockTrunk(RPPlayer player, int vehicleId)
		{
			if (!player.LoggedIn) return;

			var veh = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId);
			if (veh == null || veh.Locked) return;

			veh.SetTrunkState(!veh.TrunkLocked);
			player.Notify("Kofferraum", veh.TrunkLocked ? "Abgeschlossen." : "Aufgeschlossen.", veh.TrunkLocked ? NotificationType.ERROR : NotificationType.SUCCESS);
		}

		private static void Eject(RPPlayer player)
		{
			if (!player.LoggedIn || !player.IsInVehicle || player.Seat != 1) return;

			var veh = (RPVehicle)player.Vehicle;

			var baseModel = VehicleService.GetBase(veh.BaseId);
			if(baseModel == null) return;

			var players = RPPlayer.All.Where(x => x.Vehicle == veh);
			var data = new List<bool>();

			for(var i = 1; i <= baseModel.Seats; i++)
			{
				data.Add(players.FirstOrDefault(x => x.Seat == i) != null);
			}

			player.ShowComponent("Eject", true, JsonConvert.SerializeObject(data));
		}

		private static void Repair(RPPlayer player, int vehicleId)
		{
			if (!player.LoggedIn) return;

			var veh = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId);
			if (veh == null || (!player.IsGangwar && veh.Engine)) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var itemId = 0;

			if(player.IsGangwar && InventoryService.HasItems(player.InventoryId, 26) >= 1)
			{
				itemId = 26;
			}
			else if(player.TeamDuty && InventoryService.HasItems(player.InventoryId, 174) >= 1)
			{
				itemId = 174;
			}
			else if(InventoryService.HasItems(player.InventoryId, 8) >= 1)
			{
				itemId = 8;
			}

			if(itemId == 0)
			{
				player.Notify("Information", "Du benötigst einen Reperaturkasten!", NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(AnimationType.REPAIR);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists || veh == null || !veh.Exists) return;

				veh.Repair();

				var repairkit = InventoryService.GetItem(itemId);
				if (repairkit == null) return;
				InventoryController.RemoveItem(inventory, repairkit, 1);
			}, 10000);
		}

		private static void OpenFuel(RPPlayer player, int vehicleId)
		{
			if (!player.LoggedIn) return;

			var veh = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId);
			if (veh == null) return;

			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.GAS_STATION);
			if (shape == null) return;

			var station = GasStationService.Get(shape.Id);
			if (station == null) return;

			var baseModel = VehicleService.GetBase(veh.BaseId);
			if (baseModel == null) return;

			player.ShowComponent("GasStation", true, JsonConvert.SerializeObject(new
			{
				Id = station.Id,
				Vehicle = vehicleId,
				Max = baseModel.MaxFuel - veh.Fuel,
				Price = 20
			}));
		}

		private static void Park(RPPlayer player, int vehicleId)
		{
			if (!player.LoggedIn) return;

			var veh = RPVehicle.All.FirstOrDefault(x => x.Id == vehicleId);
			if (veh == null || !VehicleController.IsVehicleOwner(veh, player)) return;

			var garage = GetClosestGarage(veh);
			if (garage == null) return;

			VehicleController.StoreVehicle(veh, garage.Id);
		}
		
		private static GarageModel? GetClosestGarage(RPVehicle veh)
		{
			GarageModel? result = null;
			float distance = 55f;

			foreach(var garage in GarageService.GetAll())
			{
				if (garage.OwnerType != veh.OwnerType || garage.Type != veh.GarageType) continue;

				var garagePos = PositionService.Get(garage.PositionId);
				if(garagePos == null) continue;

				var dist = garagePos.Position.Distance(veh.Position);
				if(dist < distance)
				{
					result = garage;
					distance = dist;
				}
			}

			return result;
		}

		// PLAYER
		private static void GetPlayerId(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Id == targetId);
			if (target == null) return;

			player.Notify("Information", $"Spieler ID: {target.DbId}", NotificationType.INFO);
		}

		private static void GiveKey(RPPlayer player, int targetId)
		{
			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null) return;

			var warehouse = WarehouseService.GetByOwner(player.DbId, OwnerType.PLAYER);
			var house = HouseService.GetByOwner(player.DbId);

			var vehicleData = new List<GiveKeyData>();
			var vehicles = VehicleService.GetPlayerVehicles(player.DbId);
			var vehicleBases = VehicleService.GetAllBases();

			foreach(var vehicle in vehicles)
			{
				var baseModel = vehicleBases.FirstOrDefault(x => x.Id == vehicle.BaseId);
				if (baseModel == null) continue;

				vehicleData.Add(new(vehicle.Id, baseModel.Name, vehicle.KeyHolderId < 1));
			}

			player.ShowComponent("GiveKey", true, JsonConvert.SerializeObject(new
			{
				Target = target.DbId,
				House = new GiveKeyData(0, "", house != null && house.KeyHolderId < 1),
				Storage = new GiveKeyData(0, "", warehouse != null && warehouse.KeyHolderId < 1),
				Vehicles = vehicleData
			}));
		}

		private static void RevivePlayer(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn || player.TeamId != 3) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(!account.TeamDuty)
			{
				player.Notify("Information", "Du musst im Dienst sein um jemanden zu behandeln!", NotificationType.ERROR);
				return;
			}

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn || target.Alive) return;

			if (!target.Stabilized)
			{
				player.Notify("Information", "Die Person muss stabilisiert sein!", NotificationType.ERROR);
				return;
			}

			if (target.Coma)
			{
				player.Notify("Information", "Die Person ist bereits im Koma!", NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(AnimationType.USE_MEDIKIT);
			player.StartInteraction(() =>
			{
				if (target == null ) return;

				PlayerController.SetPlayerAlive(target, false);
				player.Notify("Information", "Du hast jemanden erfolgreich behandelt!", NotificationType.SUCCESS);
				target.Notify("Information", "Du wurdest medizinisch behandelt!", NotificationType.SUCCESS);
			}, 15000);
		}

		private static void SearchPlayer(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn) return;

			var targetAcc = AccountService.Get(target.DbId);
			if (targetAcc == null || (target.Alive && !targetAcc.Cuffed && !targetAcc.Roped)) return;

			var loadout = LoadoutService.GetPlayerLoadout(targetAcc.Id);
			var loadoutData = new List<object>();
			var itemBases = InventoryService.GetItems();

			foreach(var item in loadout)
			{
				var itemScript = InventoryController.ItemScripts.FirstOrDefault(x => x.Type == ItemType.WEAPON && ((WeaponItemScript)x).Hash == item.Hash && ((WeaponItemScript)x).Federal == target.TeamDuty);
				if (itemScript == null) continue;
				
				var itemBase = itemBases.FirstOrDefault(x => x.Id == itemScript.ItemId);
				if (itemBase == null) continue;

				loadoutData.Add(new
				{
					Id = item.Id,
					Name = itemBase.Name,
					Ammo = item.Ammo
				});
			}

			var inventory = InventoryService.Get(player.InventoryId);
			var container = InventoryService.Get(target.InventoryId);

			var inventoryItems = InventoryService.GetInventoryItems2(inventory.Id, container?.Id);
			var invItems = InventoryController.GetInventoryItems(inventoryItems.Items1, itemBases);
			var ctnItems = InventoryController.GetInventoryItems(inventoryItems.Items2, itemBases);

			if (target.HasBeenSearched)
			{
				player.ShowComponent("Inventory", true, JsonConvert.SerializeObject(new
				{
					Inventory = inventory,
					InventoryItems = invItems,
					Container = container,
					ContainerItems = ctnItems,
					Loadout = loadoutData,
					SearchTargetId = target.DbId
				}));
				return;
			}

			player.PlayAnimation(AnimationType.SEARCH);
			player.StartInteraction(() =>
			{
				if (player == null || target == null || targetAcc == null) return;

				target.HasBeenSearched = true;
				player.ShowComponent("Inventory", true, JsonConvert.SerializeObject(new
				{
					Inventory = inventory,
					InventoryItems = invItems,
					Container = container,
					ContainerItems = ctnItems,
					Loadout = loadoutData,
					SearchTargetId = target.DbId
				}));
			}, 10000);
		}

		private static void CuffPlayer(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn || !target.Alive) return;

			var targetAcc = AccountService.Get(target.DbId);
			if (targetAcc == null || targetAcc.Roped) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			if (!targetAcc.Cuffed && InventoryService.HasItems(player.InventoryId, 30) < 1)
			{
				player.Notify("Information", "Du benötist Handschellen!", NotificationType.ERROR);
				return;
			}
			
			PlayerController.SetPlayerCuffed(target, !targetAcc.Cuffed);
		}

		private static void RopePlayer(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn || !target.Alive) return;

			var targetAcc = AccountService.Get(target.DbId);
			if (targetAcc == null || targetAcc.Cuffed) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			if(InventoryService.HasItems(player.InventoryId, 29) < 1)
			{
				player.Notify("Information", "Du benötist ein Seil!", NotificationType.ERROR);
				return;
			}

			if (targetAcc.Roped)
			{
				var item = InventoryService.GetItem(30);
				if (item == null) return;

				InventoryController.RemoveItem(inventory, item, 1);
			}

			PlayerController.SetPlayerRoped(target, !targetAcc.Roped);
		}

		private static void GrabIntoVehicle(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn) return;

			RPVehicle? veh = null;
			float dist = 5;
			foreach(var vehicle in RPVehicle.All.ToList())
			{
				var distance = target.Position.Distance(vehicle.Position);
				if (distance > dist) continue;

				veh = vehicle;
				dist = distance;
			}

			if(veh == null)
			{
				player.Notify("Information", "Es muss ein Fahrzeug in der nähe stehen!", NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(AnimationType.USE_MEDIKIT);
			player.StartInteraction(() =>
			{
				if (player == null || target == null) return;

				var targetAcc = AccountService.Get(target.DbId);
				if (targetAcc == null || (target.Alive && !targetAcc.Cuffed && !targetAcc.Roped) || veh.Position.Distance(target.Position) > 5) return;

				target.SetIntoVehicle(veh, 2);
			}, 7500);
		}

		private static void TakeLicense(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn) return;

			var licenses = LicenseService.Get(target.LicenseId);
			if (licenses == null) return;

			player.EmitBrowser("Hud:ShowLicense", true, JsonConvert.SerializeObject(new
			{
				Name = target.Name,
				Car = licenses.Car,
				Truck = licenses.Truck,
				Heli = licenses.Heli,
				Plane = licenses.Plane,
				Boat = licenses.Boat,
				Taxi = licenses.Taxi,
				Lawyer = licenses.Lawyer,
				Gun = licenses.Gun
			}));
		}

		private static void GiveLicense(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn) return;

			var licenses = LicenseService.Get(player.LicenseId);
			if (licenses == null) return;

			target.EmitBrowser("Hud:ShowLicense", true, JsonConvert.SerializeObject(new
			{
				Name = player.Name,
				Car = licenses.Car,
				Truck = licenses.Truck,
				Heli = licenses.Heli,
				Plane = licenses.Plane,
				Boat = licenses.Boat,
				Taxi = licenses.Taxi,
				Lawyer = licenses.Lawyer,
				Gun = licenses.Gun
			}));
		}

		private static void TakeID(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn) return;

			var account = AccountService.Get(target.DbId);
			if (account == null) return;

			var custom = CustomizationService.Get(target.CustomizationId);
			if (custom == null) return;

			player.EmitBrowser("Hud:ShowIdCard", true, JsonConvert.SerializeObject(new
			{
				Name = account.Name,
				Date = account.Created,
				Level = account.Level,
				Gender = custom.Gender
			}));
		}

		private static void GiveID(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			target.EmitBrowser("Hud:ShowIdCard", true, JsonConvert.SerializeObject(new
			{
				Name = account.Name,
				Date = account.Created,
				Level = account.Level,
				Gender = custom.Gender
			}));
		}

		private static void GiveMoney(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn) return;

			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new InputClientData(
				"Geld übergabe",
				"Geben einen Betrag zum übergeben an.",
				InputType.TEXT,
				"Server:Player.GiveMoney",
				target.DbId)));
		}

		private static void StabilizePlayer(RPPlayer player, int targetId)
		{
			if (!player.LoggedIn) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Id == targetId);
			if (target == null || !target.LoggedIn) return;

			if(target.Coma)
			{
				player.Notify("Information", "Die Person ist bereits im Koma!", NotificationType.ERROR);
				return;
			}

			if (target.Stabilized)
			{
				player.Notify("Information", GetInjuryMessage(target.InjuryType), NotificationType.INFO);
				return;
			}

			var itemId = 0;

			if (player.TeamDuty && InventoryService.HasItems(player.InventoryId, 173) >= 1)
			{
				itemId = 173;
			}
			else if (InventoryService.HasItems(player.InventoryId, 10) >= 1)
			{
				itemId = 10;
			}

			if (itemId == 0)
			{
				player.Notify("Information", "Du benötigst einen Verbandskasten!", NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(AnimationType.USE_MEDIKIT);
			player.StartInteraction(() =>
			{
				if (player == null || target == null || target.Alive) return;

				if (target.Coma)
				{
					player.Notify("Information", "Die Person ist bereits im Koma!", NotificationType.INFO);
					return;
				}

				if(player.InjuryType == InjuryType.PUNCH || player.InjuryType == InjuryType.FALL_DAMAGE)
				{
					PlayerController.SetPlayerAlive(target, false);
					player.Notify("Information", "Da die Person nicht schwer verletzt war konntest du ihr direkt wieder aufhelfen!", NotificationType.INFO);
					target.Notify("Information", "Da du nicht schwer verletzt warst konnte dir jemand aufhelfen!", NotificationType.INFO);
					return;
				}

				player.Notify("Information", "Du hast jemanden Stabilisiert!", NotificationType.INFO);
				player.Notify("Information", "Du wurdest Stabilisiert!", NotificationType.INFO);
				target.Stabilized = true;
				player.SetStreamSyncedMetaData("STABILIZED", true);
			}, 20000);
		}

		private static string GetInjuryMessage(InjuryType type)
		{
			var result = "";

			switch (type)
			{
				case InjuryType.FALL_DAMAGE:
					result = "FALL DAMAGE";
					break;
				case InjuryType.PUNCH:
					result = "PUNCH!";
					break;
				case InjuryType.DROWN:
					result = "DROWN";
					break;
				case InjuryType.SLICE:
					result = "SLICE";
					break;
				case InjuryType.SHOT_LOW:
					result = "SHOT LOW";
					break;
				case InjuryType.VEHICLE:
					result = "VEHICLE";
					break;
				case InjuryType.SHOT_HIGH:
					result = "SHOT HIGH";
					break;
				case InjuryType.FIRE:
					result = "FIRE";
					break;
				case InjuryType.EXPLOSION:
					result = "EXPLOSION";
					break;
			}

			return result;
		}
	}

	public class GiveKeyData
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool Available { get; set; }

		public GiveKeyData()
		{
			Name = string.Empty;
		}

		public GiveKeyData(int id, string name, bool available)
		{
			Id = id;
			Name = name;
			Available = available;
		}
	}
}