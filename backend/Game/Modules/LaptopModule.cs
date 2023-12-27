using AltV.Net;
using AltV.Net.Data;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.Laptop;
using Database.Models.Account;
using Database.Models.Crimes;
using Database.Services;
using Game.Controllers;
using Logs;
using Logs.Enums;
using Newtonsoft.Json;

namespace Game.Modules
{
    public static class LaptopModule
	{
		public static readonly List<Unit> Units = new();
		public static readonly List<Dispatch> Dispatches = new();

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:Laptop:Open", Open);

			// ACP VEHICLES APP
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:ACPVehicles:SetPlate", ACPSetVehiclePlate);
			Alt.OnClient<RPPlayer, int, int>("Server:Laptop:ACPVehicles:ParkVehicle", ACPParkVehicle);
			Alt.OnClient<RPPlayer, int, int>("Server:Laptop:ACPVehicles:SetKeyHolder", ACPSetVehicleKeyHolder);
			Alt.OnClient<RPPlayer, int, int, int>("Server:Laptop:ACPVehicles:SetOwner", ACPSetVehicleOwner);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPVehicles:DeleteVehicle", ACPDeleteVehicle);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPVehicles:Goto", ACPGotoVehicle);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPVehicles:Bring", ACPBringVehicle);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPVehicles:RequestVehicleData", ACPRequestVehicleData);
			Alt.OnClient<RPPlayer, string>("Server:Laptop:ACPVehicles:Search", ACPVehiclesSearch);

			// ACP PLAYERS APP
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:ACPPlayers:UnwarnPlayer", ACPUnwarnPlayer);
			Alt.OnClient<RPPlayer, int, int>("Server:Laptop:ACPPlayers:SetDimension", ACPSetPlayerDimension);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPPlayers:Uncuff", ACPUncuffPlayer);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPPlayers:ToggleFreeze", ACPTogglePlayerFreeze);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPPlayers:ResetHardware", ACPResetHardware);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPPlayers:ResetSocial", ACPResetSocial);
			Alt.OnClient<RPPlayer, int, int>("Server:Laptop:ACPPlayers:SetAdmin", ACPSetAdmin);
			Alt.OnClient<RPPlayer, int, int, int>("Server:Laptop:ACPPlayers:SetMoney", ACPSetMoney);
			Alt.OnClient<RPPlayer, int, int, int>("Server:Laptop:ACPPlayers:SetTeam", ACPSetTeam);
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:ACPPlayers:UnbanPlayer", ACPUnbanPlayer);
			Alt.OnClient<RPPlayer, int, string, string, bool>("Server:Laptop:ACPPlayers:BanPlayer", ACPBanPlayer);
			Alt.OnClient<RPPlayer, int, string, bool>("Server:Laptop:ACPPlayers:KickPlayer", ACPKickPlayer);
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:ACPPlayers:WarnPlayer", ACPWarnPlayer);
			Alt.OnClient<RPPlayer, int, string, string>("Server:Laptop:ACPPlayers:SaveRecord", SaveACPRecord);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:ACPPlayers:RequestPlayerData", RequestACPPlayerData);
			Alt.OnClient<RPPlayer, string>("Server:Laptop:ACPPlayers:Search", SearchACPPlayers);

			// SUPPORT APP
			Alt.OnClient<RPPlayer>("Server:Laptop:Support:RequestData", RequestSupportData);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Support:Accept", AcceptSupportTicket);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Support:Close", CloseSupportTicket);

			// VEHICLE INFO APP
			Alt.OnClient<RPPlayer>("Server:Laptop:VehicleInfo:RequestVehicles", RequestVehicleInfo);

			// VEHICLES APP
			Alt.OnClient<RPPlayer>("Server:Laptop:Vehicles:RequestVehicles", RequestVehicles);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Vehicles:Locate", LocateVehicle);

			// CRIMES APP
			Alt.OnClient<RPPlayer>("Server:Laptop:Crimes:RequestCrimes", RequestCrimes);
			Alt.OnClient<RPPlayer, string>("Server:Laptop:Crimes:Search", SearchRecords);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Crimes:RequestPlayerData", GetPlayerData);
			Alt.OnClient<RPPlayer, int, string, string, string>("Server:Laptop:Crimes:SaveRecord", SaveRecord);
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:Crimes:AddCrimes", AddCrimes);
			Alt.OnClient<RPPlayer, int, int>("Server:Laptop:Crimes:RemoveCrime", RemoveCrime);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Crimes:RemoveAllCrimes", RemoveAllCrimes);

			// UNITS APP
			Alt.OnClient<RPPlayer>("Server:Laptop:Units:RequestData", RequestUnits);
			Alt.OnClient<RPPlayer, string, int>("Server:Laptop:Units:AddUnit", AddUnit);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Units:RemoveUnit", RemoveUnit);
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:Units:AddPlayer", AddUnitPlayer);
			Alt.OnClient<RPPlayer, int, int>("Server:Laptop:Units:RemovePlayer", RemoveUnitPlayer);
			Alt.OnClient<RPPlayer, int, int>("Server:Laptop:Units:UpdateVehicle", UpdateUnitVehicle);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Units:LocateUnit", LocateUnit);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Units:CallUnit", CallUnit);

			// DISPATCH APP
			Alt.OnClient<RPPlayer>("Server:Laptop:Dispatch:RequestData", RequestDispatches);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Dispatch:Accept", AcceptDispatch);
			Alt.OnClient<RPPlayer, int>("Server:Laptop:Dispatch:Close", CloseDispatch);
		}

		#region ACP Vehicles

		private static void ACPSetVehiclePlate(RPPlayer player, int vehId, string plate)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var vehicle = VehicleService.Get(vehId);
			if (vehicle == null) return;

			vehicle.Plate = plate;
			vehicle.Parked = true;
			VehicleService.UpdateVehicle(vehicle);

			var veh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (veh == null) return;

			veh.NumberplateText = plate;
			player.Notify("Administration", "Du hast das Kennzeichen verändert!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, vehId, TargetType.PLAYER, ACPActionType.VEHICLE_SETPLATE);
		}

		private static void ACPParkVehicle(RPPlayer player, int vehId, int garageId)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var vehicle = VehicleService.Get(vehId);
			if (vehicle == null) return;

			vehicle.GarageId = garageId;
			vehicle.Parked = true;
			VehicleService.UpdateVehicle(vehicle);

			var veh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (veh == null) return;

			veh.Delete();
			player.Notify("Administration", "Du hast ein Fahrzeug eingeparkt!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, vehId, TargetType.PLAYER, ACPActionType.VEHICLE_PARK);
		}

		private static void ACPSetVehicleKeyHolder(RPPlayer player, int vehId, int keyHolderId)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			var vehicle = VehicleService.Get(vehId);
			if (vehicle == null) return;

			vehicle.KeyHolderId = keyHolderId;
			VehicleService.UpdateVehicle(vehicle);

			var veh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (veh == null) return;

			veh.KeyHolderId = keyHolderId;
			player.Notify("Administration", "Du hast den Schlüsselbesitzer umgesetzt!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, vehId, TargetType.PLAYER, ACPActionType.VEHICLE_SETKEYHOLDER);
		}

		private static void ACPSetVehicleOwner(RPPlayer player, int vehId, int ownerId, int ownerType)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			var vehicle = VehicleService.Get(vehId);
			if (vehicle == null) return;

			vehicle.OwnerId = ownerId;
			vehicle.Type = (OwnerType)ownerType;
			VehicleService.UpdateVehicle(vehicle);

			var veh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (veh == null) return;

			veh.OwnerId = ownerId;
			veh.OwnerType = (OwnerType)ownerType;
			player.Notify("Administration", "Du hast den Owner umgesetzt!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, vehId, TargetType.PLAYER, ACPActionType.VEHICLE_SETOWNER);
		}

		private static void ACPDeleteVehicle(RPPlayer player, int vehId)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			var vehicle = VehicleService.Get(vehId);
			if (vehicle == null) return;

			vehicle.OwnerId = -1;
			VehicleService.UpdateVehicle(vehicle);

			var veh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (veh == null) return;

			veh.Delete();
			player.Notify("Administration", "Du hast ein Fahrzeug gelöscht!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, vehId, TargetType.PLAYER, ACPActionType.VEHICLE_DELETE);
		}

		private static void ACPGotoVehicle(RPPlayer player, int vehId)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var vehicle = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (vehicle == null) return;

			player.SetPosition(vehicle.Position);
			player.Notify("Administration", "Du hast dich zu einem Fahrzeug teleportiert!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, vehId, TargetType.PLAYER, ACPActionType.VEHICLE_GOTO);
		}

		private static void ACPBringVehicle(RPPlayer player, int vehId)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var vehicle = RPVehicle.All.FirstOrDefault(x => x.DbId == vehId);
			if (vehicle == null) return;

			vehicle.Position = player.Position;
			vehicle.Rotation = Rotation.Zero;
			player.Notify("Administration", "Du hast ein Fahrzeug zu dir teleportiert!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, vehId, TargetType.PLAYER, ACPActionType.VEHICLE_BRING);
		}

		private static void ACPRequestVehicleData(RPPlayer player, int vehId)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var vehicle = VehicleService.Get(vehId);
			if (vehicle == null) return;

			var vehBase = VehicleService.GetBase(vehicle.BaseId);
			if (vehBase == null) return;

			var garage = GarageService.Get(vehicle.GarageId);
			if (garage == null) return;

			player.EmitBrowser("Laptop:ACPVehicles:SetVehicleData", JsonConvert.SerializeObject(new
			{
				Id = vehicle.Id,
				Name = vehBase.Name,
				Note = vehicle.Note,
				Plate = vehicle.Plate,
				Fuel = vehicle.Fuel,
				MaxFuel = vehBase.MaxFuel,
				GarageId = vehicle.GarageId,
				GarageName = garage.Name,
				Parked = vehicle.Parked
			}));
		}

		private static void ACPVehiclesSearch(RPPlayer player, string search)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var bases = VehicleService.GetAllBases();
			var teams = TeamService.GetAll();
			var vehicles = VehicleService.Search(search, bases, teams, 10);
			var data = new List<object>();

			foreach(var vehicle in vehicles)
			{
				var vehBase = bases.FirstOrDefault(x => x.Id == vehicle.BaseId);
				if (vehBase == null) continue;

				var ownerName = vehicle.Type == OwnerType.TEAM ? teams.FirstOrDefault(x => x.Id == vehicle.OwnerId)?.Name : vehicle.Type == OwnerType.PLAYER ? AccountService.Get(vehicle.OwnerId)?.Name : "SWAT";
				var keyHolder = vehicle.Type == OwnerType.PLAYER ? AccountService.Get(vehicle.KeyHolderId)?.Name : "NONE";

				data.Add(new
				{
					Id = vehicle.Id,
					Name = vehBase.Name,
					Owner = ownerName,
					KeyHolder = keyHolder,
					Plate = vehicle.Plate,
					Parked = vehicle.Parked
				});
			}

			player.EmitBrowser("Laptop:ACPVehicles:SetData", JsonConvert.SerializeObject(data));
		}

		#endregion

		#region ACP Players

		private static void ACPUnwarnPlayer(RPPlayer player, int id, string reason)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			var warns = WarnService.GetPlayerWarns(id);
			if (warns.Count < 1) return;

			WarnService.Remove(warns[0]);

			player.Notify("Administration", $"Du hast einen Warn entfernt!", NotificationType.SUCCESS);

			var history = new AdminHistoryModel(id, reason, player.DbId, player.Name, DateTime.Now, AdminHistoryType.UNWARN);
			AccountService.AddAdminHistory(history);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_UNWARN);
		}

		private static void ACPSetPlayerDimension(RPPlayer player, int id, int dimension)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			target.SetDimension(dimension);

			target.Notify("Information", $"Du wurdest von {player.Name} in Dimesion {dimension} gesetzt.", NotificationType.INFO);
			player.Notify("Administration", $"Du hast {target.Name} in Dimesion {dimension} gesetzt.", NotificationType.INFO);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_SETDIMENSION);
		}

		private static void ACPUncuffPlayer(RPPlayer player, int id)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			PlayerController.SetPlayerCuffed(target, false);
			PlayerController.SetPlayerRoped(target, false);

			target.Notify("Information", $"Du wurdest von {player.Name} entfesselt.", NotificationType.INFO);
			player.Notify("Administration", $"Du hast {target.Name} entfesselt.", NotificationType.INFO);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_UNCUFF);
		}

		private static void ACPTogglePlayerFreeze(RPPlayer player, int id)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			target.Frozen = !target.Frozen;
			target.Notify("Information", $"Du wurdest von {player.Name} {(target.Frozen ? "gefreezed" : "unfreezed")}.", NotificationType.INFO);
			player.Notify("Administration", $"Du hast {target.Name} {(target.Frozen ? "gefreezed" : "unfreezed")}.", NotificationType.INFO);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_TOGGLE_FREEZE);
		}

		private static void ACPResetHardware(RPPlayer player, int id)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.HardwareId = 0;
			targetAccount.HardwareIdEx = 0;
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast den Socialclub von {targetAccount.Name} zurück gesetzt!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_HWID_RESET);
		}

		private static void ACPResetSocial(RPPlayer player, int id)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.SocialclubId = 0;
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast den Socialclub von {targetAccount.Name} zurück gesetzt!", NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_SCID_RESET);
		}

		private static void ACPSetAdmin(RPPlayer player, int id, int rank)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN || rank > Enum.GetValues<AdminRank>().Length) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.AdminRank = (AdminRank)rank;
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast {targetAccount.Name} auf ${rank} gesetzt!", NotificationType.SUCCESS);

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			target.AdminRank = (AdminRank)rank;
			target.Notify("Administration", $"Du wurdest von {player.Name} auf Rang {rank} gesetzt!", Core.Enums.NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_SETADMIN);
		}

		private static void ACPSetMoney(RPPlayer player, int id, int money, int bank)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.Money = money;
			targetAccount.BankMoney = bank;
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast das Geld von {targetAccount.Name} auf ${money} und ${bank} gesetzt!", Core.Enums.NotificationType.SUCCESS);

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			target.EmitBrowser("Hud:SetMoney", money);
			target.Notify("Administration", $"Dein Geld wurde von {player.Name} auf ${money} und ${bank} gesetzt!", Core.Enums.NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_SETMONEY);
		}

		private static void ACPSetTeam(RPPlayer player, int id, int teamId, int rank)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var team = TeamService.Get(teamId);
			if (team == null) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.TeamId = teamId;
			targetAccount.TeamRank = rank;
			targetAccount.TeamAdmin = rank >= 10;
			targetAccount.TeamStorage = rank >= 10;
			targetAccount.TeamBank = rank >= 10;
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast {targetAccount.Name} in die Fraktion {team.ShortName} gesetzt!", NotificationType.SUCCESS);

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			target.TeamId = teamId;
			target.Notify("Administration", $"Du wurdest von {player.Name} in die Fraktion {team.ShortName} gesetzt!", Core.Enums.NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_SETTEAM);
		}

		private static void ACPUnbanPlayer(RPPlayer player, int id, string reason)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.BanReason = $"Entbannt von {player.Name}: {reason}";
			targetAccount.BannedUntil = DateTime.Now;
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast {targetAccount.Name} entbannt!", NotificationType.SUCCESS);

			var history = new AdminHistoryModel(targetAccount.Id, reason, player.DbId, player.Name, DateTime.Now, AdminHistoryType.UNBAN);
			AccountService.AddAdminHistory(history);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_UNBAN);
		}

		private static void ACPBanPlayer(RPPlayer player, int id, string reason, string datetime, bool anonym)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.BanReason = reason;
			targetAccount.BannedUntil = DateTime.Parse(datetime);
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast {targetAccount.Name} vom Server gebannt!", NotificationType.SUCCESS);

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			if(!anonym)
			{
				var data = JsonConvert.SerializeObject(new
				{
					Title = $"ADMINISTRATIVE NACHRICHT",
					Message = $"{player.Name} hat {target.Name} vom Server gebannt! Grund: {reason}",
					Duration = 10000
				});

				foreach (var user in RPPlayer.All.ToList())
				{
					user.EmitBrowser("Hud:ShowGlobalNotification", data);
				}
			}

			var history = new AdminHistoryModel(target.DbId, reason, player.DbId, player.Name, DateTime.Now, AdminHistoryType.BAN);
			AccountService.AddAdminHistory(history);

			target.Kick($"Du wurdest vom Gameserver gebannt! Grund: {reason}");
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_BAN);
		}

		private static void ACPKickPlayer(RPPlayer player, int id, string reason, bool anonym)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if(target == null)
			{
				player.Notify("Administration", "Der Spieler ist nicht Online!", Core.Enums.NotificationType.ERROR);
				return;
			}

			player.Notify("Administration", $"Du hast {target.Name} vom Server gekickt!", Core.Enums.NotificationType.SUCCESS);

			if(!anonym)
			{
				var data = JsonConvert.SerializeObject(new
				{
					Title = $"ADMINISTRATIVE NACHRICHT",
					Message = $"{player.Name} hat {target.Name} vom Server gekickt! Grund: {reason}",
					Duration = 10000
				});

				foreach (var user in RPPlayer.All.ToList())
				{
					user.EmitBrowser("Hud:ShowGlobalNotification", data);
				}
			}

			var history = new AdminHistoryModel(target.DbId, reason, player.DbId, player.Name, DateTime.Now, AdminHistoryType.KICK);
			AccountService.AddAdminHistory(history);

			target.Kick($"Du wurdest vom Gameserver gekickt! Grund: {reason}");
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_KICK);
		}

		private static void ACPWarnPlayer(RPPlayer player, int id, string reason)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var account = AccountService.Get(id);
			if (account == null) return;

			var warn = new WarnModel(account.Id, reason, player.DbId, DateTime.Now);
			WarnService.Add(warn);
			player.Notify("Administration", $"Du hast {account.Name} verwarnt!", NotificationType.SUCCESS);

			var history = new AdminHistoryModel(account.Id, reason, player.DbId, player.Name, DateTime.Now, AdminHistoryType.WARN);
			AccountService.AddAdminHistory(history);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_WARN);
		}

		private static void SaveACPRecord(RPPlayer player, int id, string description, string supportCallMessage)
		{
			if (player.AdminRank < Core.Enums.AdminRank.MODERATOR) return;

			var account = AccountService.Get(id);
			if (account == null) return;

			account.AdminRecordDescription = description;
			account.SupportCallMessage = supportCallMessage;
			AccountService.Update(account);

			player.Notify("Administration", $"Du hast die Beschreibung von {account.Name} bearbeitet!", Core.Enums.NotificationType.SUCCESS);
			LogService.LogACPAction(player.DbId, id, TargetType.PLAYER, ACPActionType.PLAYER_SAVEDATA);
		}

		private static void RequestACPPlayerData(RPPlayer player, int id)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPPORTER) return;

			var account = AccountService.Get(id);
			if (account == null) return;

			var house = HouseService.GetByOwner(id);
			var warehouse = WarehouseService.GetByOwner(id, Core.Enums.OwnerType.PLAYER);

			var team = TeamService.Get(account.TeamId);

			var history = AccountService.GetPlayerAdminHistory(account.Id);
			var historyData = new List<object>();

			foreach(var model in history)
			{
				historyData.Add(new
				{
					Id = model.Id,
					Reason = model.Reason,
					AdminId = model.AdminId,
					AdminName = model.AdminName,
					Date = model.Date.ToString("HH:mm dd.MM.yyyy"),
					Type = model.Type
				});
			}

			var data = new
			{
				Id = account.Id,
				Name = account.Name,
				House = house?.Id,
				Warehouse = warehouse?.Id,
				TeamId = account.TeamId,
				TeamName = team?.ShortName,
				TeamRank = account.TeamRank,
				BusinessId = account.BusinessId,
				BusinessName = "",
				BusinessRank = 0,
				Phone = account.PhoneNumber,
				Description = account.AdminRecordDescription,
				SupportCallMessage = account.SupportCallMessage,
				Rank = account.AdminRank.ToString(),
				LastOnline = account.LastOnline.ToString("HH:mm dd.MM.yyyy"),
				Warns = WarnService.GetPlayerWarnsCount(account.Id),
				Money = account.Money,
				BankMoney = account.BankMoney,
				AdminHistory = historyData
			};

			player.EmitBrowser("Laptop:ACPPlayers:SetUserData", JsonConvert.SerializeObject(data));
		}

		private static void SearchACPPlayers(RPPlayer player, string search)
		{
			if (player.AdminRank < AdminRank.SUPPORTER || search.Length < 1) return;

			var accounts = AccountService.Search(search, 10);
			var data = new List<object>();
			var onlinePlayers = RPPlayer.All.ToList();

			foreach(var account in accounts)
			{
				data.Add(new
				{
					Id = account.Id,
					Name = account.Name,
					Team = account.TeamId,
					Rank = account.AdminRank.ToString(),
					Phone = account.PhoneNumber,
					LastOnline = account.LastOnline.ToString("HH:mm dd.MM.yyyy"),
					Warns = WarnService.GetPlayerWarnsCount(account.Id),
					Online = onlinePlayers.Any(x => x.DbId == account.Id)
				});
			}

			player.EmitBrowser("Laptop:ACPPlayers:SetData", JsonConvert.SerializeObject(data));
		}

		#endregion

		#region Support

		private static void RequestSupportData(RPPlayer player)
		{
			player.EmitBrowser("Laptop:Support:SetData", JsonConvert.SerializeObject(SupportModule.Tickets));
		}

		private static void AcceptSupportTicket(RPPlayer player, int ticketId)
		{
			var ticket = SupportModule.Tickets.FirstOrDefault(x => x.Id == ticketId);
			if (ticket == null || ticket.AdminId > 0)
			{
				player.ShowComponent("Laptop", false);
				return;
			}

			var creator = RPPlayer.All.FirstOrDefault(x => x.DbId == ticket.CreatorId);
			if (creator == null)
			{
				SupportModule.Tickets.Remove(ticket);
				player.ShowComponent("Laptop", false);
				player.Notify("Ticket System", "Der Spieler ist nicht mehr online! Das Ticket wurde gelöscht.", Core.Enums.NotificationType.ERROR);
				return;
			}

			ticket.AdminId = player.DbId;
			ticket.Admin = player.Name;
			player.Notify("Information", $"Du hast das Ticket von {creator.Name} angenommen!", Core.Enums.NotificationType.INFO);
			creator.Notify("Information", $"Dein Ticket wurde von {player.Name} angenommen!", Core.Enums.NotificationType.INFO);
		}

		private static void CloseSupportTicket(RPPlayer player, int ticketId)
		{
			var ticket = SupportModule.Tickets.FirstOrDefault(x => x.Id == ticketId);
			if (ticket == null)
			{
				player.ShowComponent("Laptop", false);
				return;
			}

			var creator = RPPlayer.All.FirstOrDefault(x => x.DbId == ticket.CreatorId);
			if (creator == null)
			{
				SupportModule.Tickets.Remove(ticket);
				player.ShowComponent("Laptop", false);
				player.Notify("Ticket System", "Der Spieler ist nicht mehr online! Das Ticket wurde gelöscht.", Core.Enums.NotificationType.ERROR);
				return;
			}

			SupportModule.Tickets.Remove(ticket);
			player.Notify("Information", $"Du hast das Ticket von {creator.Name} geschlossen!", Core.Enums.NotificationType.INFO);
			creator.Notify("Information", $"Dein Ticket wurde von {player.Name} geschlossen!", Core.Enums.NotificationType.INFO);
		}

		#endregion

		#region VehicleInfo

		private static void RequestVehicleInfo(RPPlayer player)
		{
			var bases = VehicleService.GetAllBases();
			var data = new List<object>();
			foreach(var veh in bases)
			{
				data.Add(new
				{
					Id = veh.Id,
					Name = veh.Name,
					TrunkWeight = veh.TrunkWeight,
					TrunkSlots = veh.TrunkSlots,
					Fuel = veh.MaxFuel,
					Tax = veh.Tax,
					Price = veh.Price
				});
			}

			player.EmitBrowser("Laptop:VehicleInfo:SetData", JsonConvert.SerializeObject(data));
		}

		#endregion

		#region Vehicles 

		private static void RequestVehicles(RPPlayer player)
		{
			var garages = GarageService.GetAll();
			var bases = VehicleService.GetAllBases();
			var vehicles = VehicleService.GetAllPlayerVehicles(player.DbId, player.TeamId, player.BusinessId);
			var data = new List<object>();

			foreach(var vehicle in vehicles)
			{
				var garage = garages.FirstOrDefault(x => x.Id == vehicle.GarageId);
				if (garage == null) continue;

				var vehBase = bases.FirstOrDefault(x => x.Id == vehicle.BaseId);
				if (vehBase == null) continue;

				data.Add(new
				{
					Id = vehicle.Id,
					Name = vehBase.Name,
					Plate = vehicle.Plate,
					Note = vehicle.Note,
					Garage = garage.Name,
					Type = (vehicle.Type == Core.Enums.OwnerType.TEAM ? 2 : vehicle.OwnerId == player.DbId ? 0 : 1)
				});
			}

			player.EmitBrowser("Laptop:Vehicles:SetData", JsonConvert.SerializeObject(data));
		}

		private static void LocateVehicle(RPPlayer player, int vehicleId)
		{
			var veh = RPVehicle.All.FirstOrDefault(x => x.DbId == vehicleId);
			if(veh != null)
			{
				player.Emit("Client:PlayerModule:SetWaypoint", veh.Position.X, veh.Position.Y);
				player.Notify("Information", "Du hast ein Fahrzeug geortet!", Core.Enums.NotificationType.SUCCESS);
				return;
			}

			var vehicle = VehicleService.Get(vehicleId);
			if (vehicle == null) return;

			var garage = GarageService.Get(vehicle.GarageId);
			if (garage == null) return;

			var pos = PositionService.Get(garage.PositionId);
			if (pos == null) return;

			player.Emit("Client:PlayerModule:SetWaypoint", pos.Position.X, pos.Position.Y);
			player.Notify("Information", "Du hast ein Fahrzeug geortet!", Core.Enums.NotificationType.SUCCESS);
		}

		#endregion

		#region Crimes

		private static void RequestCrimes(RPPlayer player)
		{
			if (player.TeamId < 1 || (player.TeamId > 2 && player.TeamId != 5)) return;

			var crimes = CrimeService.GetAll();
			var groups = CrimeService.GetAllGroups();

			player.EmitBrowser("Laptop:Crimes:SetCrimes", JsonConvert.SerializeObject(new
			{
				Crimes = crimes,
				Groups = groups
			}));
		}

		private static void SearchRecords(RPPlayer player, string targetName)
		{
			if (player.TeamId < 1 || (player.TeamId > 2 && player.TeamId != 5)) return;

			var targets = AccountService.GetFromName(targetName, 12);
			var data = new List<object>();
			foreach (var target in targets)
			{
				data.Add(new
				{
					Id = target.Id,
					Name = target.Name,
					Jailtime = target.Jailtime
				});
			}

			player.EmitBrowser("Laptop:Crimes:SetPlayers", JsonConvert.SerializeObject(data));
		}

		private static void GetPlayerData(RPPlayer player, int targetId)
		{
			if (player.TeamId < 1 || (player.TeamId > 2 && player.TeamId != 5)) return;

			var target = AccountService.Get(targetId);
			if (target == null) return;

			var crimes = CrimeService.GetPlayerCrimes(target.Id);
			var house = HouseService.GetByOwner(target.Id);
			var warehouse = WarehouseService.GetByOwner(target.Id, Core.Enums.OwnerType.PLAYER);

			player.EmitBrowser("Laptop:Crimes:SetUserData", JsonConvert.SerializeObject(new
			{
				Id = target.Id,
				Name = target.Name,
				House = house == null ? 0 : house.Id,
				Warehouse = warehouse == null ? 0 : warehouse.Id,
				Team = target.FederalRecordTeam,
				Description = target.FederalRecordDescription,
				Phone = target.FederalRecordPhone,
				Crimes = crimes,
				Jailtime = target.Jailtime
			}));
		}

		private static void SaveRecord(RPPlayer player, int targetId, string team, string description, string phone)
		{
			if (player.TeamId < 1 || (player.TeamId > 2 && player.TeamId != 5)) return;

			if (!CheckString(description))
			{
				player.ShowComponent("Laptop", false);
				player.Notify("Information", "Die Beschreibung darf keine Sonderzeichen enthalten (Ä/Ö/Ü)", Core.Enums.NotificationType.ERROR);
				return;
			}

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(account.TeamRank < 7)
			{
				player.Notify("Information", "Du musst mind. Rang 7 dafür sein!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var target = AccountService.Get(targetId);
			if (target == null) return;

			target.FederalRecordTeam = team;
			target.FederalRecordDescription = description;
			target.FederalRecordPhone = phone;
			AccountService.Update(target);
			TeamController.Broadcast(new List<int>() { 1, 2, 5 }, $"{player.Name} hat die Akte von {target.Name} bearbeitet!", Core.Enums.NotificationType.INFO);
		}

		private static void AddCrimes(RPPlayer player, int targetId, string json)
		{
			if (player.TeamId < 1 || (player.TeamId > 2 && player.TeamId != 5)) return;

			var target = AccountService.Get(targetId);
			if (target == null) return;

			var crimes = JsonConvert.DeserializeObject<List<int>>(json);
			if(crimes == null || crimes.Count < 1) return;

			var now = DateTime.Now;
			var dateLabel = DateTime.Now.ToString("HH:mm dd.MM.yyyy");

			var crimesToAdd = new List<CrimeModel>();
			foreach (var crime in crimes)
			{
				crimesToAdd.Add(new(targetId, crime, dateLabel, player.Name));
			}

			CrimeService.Add(crimesToAdd);
			TeamController.Broadcast(new List<int>() { 1, 2, 5 }, $"{player.Name} hat die Akte von {target.Name} bearbeitet!", Core.Enums.NotificationType.INFO);

			var data = CrimeService.GetPlayerCrimes(target.Id);
			player.EmitBrowser("Laptop:Crimes:SetCrimesData", JsonConvert.SerializeObject(data));
		}

		private static void RemoveCrime(RPPlayer player, int targetId, int crimeId)
		{
			if (player.TeamId < 1 || (player.TeamId > 2 && player.TeamId != 5)) return;

			var target = AccountService.Get(targetId);
			if (target == null) return;

			var crime = CrimeService.Get(crimeId);
			if (crime == null) return;

			CrimeService.Remove(crime);
			TeamController.Broadcast(new List<int>() { 1, 2, 5 }, $"{player.Name} hat {target.Name} eine Akte erlassen!", Core.Enums.NotificationType.INFO);
		}

		private static void RemoveAllCrimes(RPPlayer player, int targetId)
		{
			if (player.TeamId < 1 || (player.TeamId > 2 && player.TeamId != 5)) return;

			var target = AccountService.Get(targetId);
			if (target == null) return;

			CrimeService.RemovePlayerCrimes(target.Id);
			TeamController.Broadcast(new List<int>() { 1, 2, 5 }, $"{player.Name} hat {target.Name} alle Akten erlassen!", Core.Enums.NotificationType.INFO);
		}
		#endregion

		#region Units

		private static void RequestUnits(RPPlayer player)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			var units = Units.Where(x => x.Team == player.TeamId);
			player.EmitBrowser("Laptop:Units:SetData", JsonConvert.SerializeObject(units));
		}

		private static void AddUnit(RPPlayer player, string name, int vehicleId)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			var unit = new Unit(name, vehicleId, new(), player.TeamId);
			Units.Add(unit);

			var data = JsonConvert.SerializeObject(unit);
			foreach(var target in RPPlayer.All.Where(x => x.TeamId == player.TeamId).ToList())
			{
				target.EmitBrowser("Laptop:Units:UpdateUnit", data);
			}

			player.Notify("Information", "Du hast eine Einheit erstellt!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void RemoveUnit(RPPlayer player, int unitId)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			var unit = Units.FirstOrDefault(x => x.Id == unitId);
			if (unit == null || player.TeamId != unit.Team) return;

			var data = JsonConvert.SerializeObject(unit);
			Units.Remove(unit);

			foreach (var target in RPPlayer.All.Where(x => x.TeamId == player.TeamId).ToList())
			{
				target.EmitBrowser("Laptop:Units:RemoveUnit", data);
			}

			player.Notify("Information", "Du hast eine Einheit entfernt!", Core.Enums.NotificationType.INFO);
		}

		private static void AddUnitPlayer(RPPlayer player, int unitId, string targetInput)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			var unit = Units.FirstOrDefault(x => x.Id == unitId);
			if (unit == null || player.TeamId != unit.Team) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Name.ToLower() == targetInput.ToLower());
			if (target == null || target.TeamId != unit.Team) return;

			var targetAccount = AccountService.Get(target.DbId);
			if (targetAccount == null) return;

			unit.Members.Add(new(target.DbId, target.Name, targetAccount.TeamRank));

			var data = JsonConvert.SerializeObject(unit);
			foreach (var teamMember in RPPlayer.All.Where(x => x.TeamId == player.TeamId).ToList())
			{
				teamMember.EmitBrowser("Laptop:Units:UpdateUnit", data);
			}

			player.Notify("Information", $"Du hast {target.Name} zu {unit.Name} hinzugefügt!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void RemoveUnitPlayer(RPPlayer player, int unitId, int targetId)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			var unit = Units.FirstOrDefault(x => x.Id == unitId);
			if (unit == null || player.TeamId != unit.Team) return;

			var target = unit.Members.FirstOrDefault(x => x.Id == targetId);
			if (target == null) return;

			unit.Members.Remove(target);

			var data = JsonConvert.SerializeObject(unit);
			foreach (var teamMember in RPPlayer.All.Where(x => x.TeamId == player.TeamId).ToList())
			{
				teamMember.EmitBrowser("Laptop:Units:UpdateUnit", data);
			}

			player.Notify("Information", $"Du hast {target.Name} aus {unit.Name} entfernt!", Core.Enums.NotificationType.INFO);
		}

		private static void UpdateUnitVehicle(RPPlayer player, int unitId, int vehicleId)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			var unit = Units.FirstOrDefault(x => x.Id == unitId);
			if (unit == null || player.TeamId != unit.Team) return;

			unit.Vehicle = vehicleId;

			var data = JsonConvert.SerializeObject(unit);
			foreach (var teamMember in RPPlayer.All.Where(x => x.TeamId == player.TeamId).ToList())
			{
				teamMember.EmitBrowser("Laptop:Units:UpdateUnit", data);
			}

			player.Notify("Information", $"Du hast das Fahrzeug von {unit.Name} bearbeitet!", Core.Enums.NotificationType.INFO);
		}

		private static void LocateUnit(RPPlayer player, int unitId)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			var unit = Units.FirstOrDefault(x => x.Id == unitId);
			if (unit == null || player.TeamId != unit.Team) return;

			var vehicle = RPVehicle.All.FirstOrDefault(x => x.DbId == unit.Vehicle);
			if (vehicle == null || vehicle.OwnerType != Core.Enums.OwnerType.TEAM || vehicle.OwnerId != player.TeamId) return;

			player.Emit("Client:PlayerModule:SetWaypoint", vehicle.Position.X, vehicle.Position.Y);
			player.Notify("Information", $"Du hast {unit.Name} lokalisiert!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void CallUnit(RPPlayer player, int unitId)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			var unit = Units.FirstOrDefault(x => x.Id == unitId);
			if (unit == null || player.TeamId != unit.Team) return;

			foreach(var member in unit.Members)
			{
				var target = RPPlayer.All.FirstOrDefault(x => x.DbId == member.Id);
				if (target == null) continue;

				target.Emit("Client:PlayerModule:SetWaypoint", player.Position.X, player.Position.Y);
				target.Notify("Information", $"Deine Einheit wurde von {player.Name} angefordert!", Core.Enums.NotificationType.WARN);
			}

			player.Notify("Information", $"Du hast {unit.Name} angefordert!", Core.Enums.NotificationType.SUCCESS);
		}

		#endregion

		#region Dispatch

		private static void RequestDispatches(RPPlayer player)
		{
			if (player.TeamId < 1 || player.TeamId > 5) return;

			player.EmitBrowser("Laptop:Dispatch:SetData", JsonConvert.SerializeObject(Dispatches.Where(x => HasAccessToDispatch(x, player.TeamId))));
		}

		private static void AcceptDispatch(RPPlayer player, int dispatchId)
		{
			var dispatch = Dispatches.FirstOrDefault(x => x.Id == dispatchId);
			if (dispatch == null || dispatch.Officer != "") return;

			var creator = RPPlayer.All.FirstOrDefault(x => x.DbId == dispatch.Id);
			if (creator != null)
			{
				creator.Notify("Information", "Dein Dispatch wurde angenommen!", Core.Enums.NotificationType.INFO);
			}

			dispatch.Officer = player.Name;

			TeamController.Broadcast(player.TeamId, $"{player.Name} hat den Dispatch von {dispatch.Creator} angenommen!", Core.Enums.NotificationType.INFO);
		}

		private static void CloseDispatch(RPPlayer player, int dispatchId)
		{
			var dispatch = Dispatches.FirstOrDefault(x => x.Id == dispatchId);
			if (dispatch == null) return;

			var creator = RPPlayer.All.FirstOrDefault(x => x.DbId == dispatch.Id);
			if (creator != null)
			{
				creator.Notify("Information", "Dein Dispatch wurde geschlossen!", Core.Enums.NotificationType.INFO);
			}

			Dispatches.Remove(dispatch);

			TeamController.Broadcast(player.TeamId, $"{player.Name} hat den Dispatch von {dispatch.Creator} geschlossen!", Core.Enums.NotificationType.INFO);
		}

		#endregion

		private static void Open(RPPlayer player)
		{
			if (!player.Laptop) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.PlayAnimation(Core.Enums.AnimationType.LAPTOP);
			player.ShowComponent("Laptop", true, JsonConvert.SerializeObject(new
			{
				Name = player.Name,
				Team = account.TeamId,
				TeamLeader = account.TeamAdmin,
				Admin = (int)player.AdminRank
			}));
		}

		public static bool HasAccessToDispatch(Dispatch dispatch, int team)
		{
			if (dispatch.Team == 1 && team == 2) return true;

			return dispatch.Team == team;
		}

		private static bool CheckString(string str)
		{
			foreach(char c in str)
			{
				if (!Char.IsLetterOrDigit(c) && c != '_' && c != '-' && c != '/' && c != '(' && c != ')')
					return false;
			}

			return true;
		}
	}
}