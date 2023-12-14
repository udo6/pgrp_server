﻿using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.Laptop;
using Database.Models.Crimes;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;
using System;

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

			// ACP PLAYERS APP
			Alt.OnClient<RPPlayer, int, int>("Server:Laptop:ACPPlayers:SetAdmin", ACPSetAdmin);
			Alt.OnClient<RPPlayer, int, int, int>("Server:Laptop:ACPPlayers:SetMoney", ACPSetMoney);
			Alt.OnClient<RPPlayer, int, int, int>("Server:Laptop:ACPPlayers:SetTeam", ACPSetTeam);
			Alt.OnClient<RPPlayer, int, string, string>("Server:Laptop:ACPPlayers:BanPlayer", ACPBanPlayer);
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:ACPPlayers:KickPlayer", ACPKickPlayer);
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:ACPPlayers:WarnPlayer", ACPWarnPlayer);
			Alt.OnClient<RPPlayer, int, string>("Server:Laptop:ACPPlayers:SaveRecord", SaveACPRecord);
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

		#region ACP Players

		private static void ACPSetAdmin(RPPlayer player, int id, int rank)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN || rank > Enum.GetValues<AdminRank>().Length) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.AdminRank = (AdminRank)rank;
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast {targetAccount.Name} auf ${rank} gesetzt!", Core.Enums.NotificationType.SUCCESS);

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			target.AdminRank = (AdminRank)rank;
			target.Notify("Administration", $"Du wurdest von {player.Name} auf Rang {rank} gesetzt!", Core.Enums.NotificationType.SUCCESS);
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

			player.Notify("Administration", $"Du hast {targetAccount.Name} in die Fraktion {team.ShortName} gesetzt!", Core.Enums.NotificationType.SUCCESS);

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			target.TeamId = teamId;
			target.Notify("Administration", $"Du wurdest von {player.Name} in die Fraktion {team.ShortName} gesetzt!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void ACPBanPlayer(RPPlayer player, int id, string reason, string datetime)
		{
			if (player.AdminRank < Core.Enums.AdminRank.MODERATOR) return;

			var targetAccount = AccountService.Get(id);
			if (targetAccount == null) return;

			targetAccount.BanReason = reason;
			targetAccount.BannedUntil = DateTime.Parse(datetime);
			AccountService.Update(targetAccount);

			player.Notify("Administration", $"Du hast {targetAccount.Name} vom Server gebannt!", Core.Enums.NotificationType.SUCCESS);

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (target == null) return;

			var data = JsonConvert.SerializeObject(new
			{
				Title = $"ADMINISTRATIVE NACHRICHT",
				Message = $"{player.Name} hat {target.Name} vom Server gebannt! Grund: {reason}",
				Duration = 10000
			});

			target.Kick($"Du wurdest von {player.Name} gebannt! Grund: {reason}");

			foreach (var user in RPPlayer.All.ToList())
			{
				user.EmitBrowser("Hud:ShowGlobalNotification", data);
			}

			// add history
		}

		private static void ACPKickPlayer(RPPlayer player, int id, string reason)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if(target == null)
			{
				player.Notify("Administration", "Der Spieler ist nicht Online!", Core.Enums.NotificationType.ERROR);
				return;
			}

			player.Notify("Administration", $"Du hast {target.Name} vom Server gekickt!", Core.Enums.NotificationType.SUCCESS);

			var data = JsonConvert.SerializeObject(new
			{
				Title = $"ADMINISTRATIVE NACHRICHT",
				Message = $"{player.Name} hat {target.Name} vom Server gekickt! Grund: {reason}",
				Duration = 10000
			});

			target.Kick($"Du wurdest von {player.Name} gekickt! Grund: {reason}");

			foreach (var user in RPPlayer.All.ToList())
			{
				user.EmitBrowser("Hud:ShowGlobalNotification", data);
			}
			// add history
		}

		private static void ACPWarnPlayer(RPPlayer player, int id, string reason)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPPORTER) return;

			// warn stuff
		}

		private static void SaveACPRecord(RPPlayer player, int id, string description)
		{
			if (player.AdminRank < Core.Enums.AdminRank.MODERATOR) return;

			var account = AccountService.Get(id);
			if (account == null) return;

			account.AdminRecordDescription = description;
			AccountService.Update(account);

			player.Notify("Administration", $"Du hast die Beschreibung von {account.Name} bearbeitet!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void RequestACPPlayerData(RPPlayer player, int id)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPPORTER) return;

			var account = AccountService.Get(id);
			if (account == null) return;

			var house = HouseService.GetByOwner(id);
			var warehouse = WarehouseService.GetByOwner(id, Core.Enums.OwnerType.PLAYER);

			var team = TeamService.Get(account.TeamId);

			var data = new
			{
				// Todo: add warns

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
				Rank = account.AdminRank.ToString(),
				LastOnline = account.LastOnline.ToString("HH:mm dd.MM.yyyy"),
				Warns = 0,
				Money = account.Money,
				BankMoney = account.BankMoney,
				AdminHistory = new List<object>()
			};

			player.EmitBrowser("Laptop:ACPPlayers:SetUserData", JsonConvert.SerializeObject(data));
		}

		private static void SearchACPPlayers(RPPlayer player, string search)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPPORTER || search.Length < 1) return;

			var accounts = AccountService.Search(search, 10);
			var data = new List<object>();
			foreach(var account in accounts)
			{
				data.Add(new
				{
					// Todo: add warns

					Id = account.Id,
					Name = account.Name,
					Team = account.TeamId,
					Rank = account.AdminRank.ToString(),
					Phone = account.PhoneNumber,
					LastOnline = account.LastOnline.ToString("HH:mm dd.MM.yyyy"),
					Warns = 0
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
				Admin = player.AdminRank > Core.Enums.AdminRank.GUIDE && player.AdminDuty
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