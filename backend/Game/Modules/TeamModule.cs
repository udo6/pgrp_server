﻿using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Game.Controllers;
using Core.Enums;
using Database.Services;
using Backend.Utils.Models.Team.Client;
using Newtonsoft.Json;
using Database.Models.Account;
using Database.Models.Team;
using Database.Models.Bank;
using AltV.Net.Enums;

namespace Game.Modules
{
    public static class TeamModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var team in TeamService.GetAll())
				TeamController.LoadTeam(team);

			Alt.OnClient<RPPlayer, int>("Server:Team:AcceptInvite", AcceptInvite);
			Alt.OnClient<RPPlayer>("Server:Team:ToggleDuty", ToggleDuty);
			Alt.OnClient<RPPlayer>("Server:Team:BuyEquip", BuyEquip);
			Alt.OnClient<RPPlayer, int, int, bool, bool, bool>("Server:Team:UpdateMember", UpdateMember);
			Alt.OnClient<RPPlayer, int, bool>("Server:Team:KickMember", KickMember);
			Alt.OnClient<RPPlayer, int>("Server:Team:TakeMoney", WithdrawMoney);
			Alt.OnClient<RPPlayer, int>("Server:Team:GiveMoney", DepositMoney);
			Alt.OnClient<RPPlayer>("Server:Team:Interact", OpenTeamMenu);
		}

		private static void AcceptInvite(RPPlayer player, int team)
		{
			if (!player.LoggedIn || team < 1) return;

			/*if(player.Level < 3)
			{
				player.Notify("Information", "Du musst mind. Level 3 sein!", NotificationType.ERROR);
				return;
			}*/

			TeamController.SetPlayerTeam(player, team, 0, false, false, false);
			TeamController.Broadcast(team, $"{player.Name} ist der Fraktion beigetreten!", NotificationType.INFO);
		}

		private static void ToggleDuty(RPPlayer player)
		{
			if (!player.LoggedIn || player.TeamId < 1) return;

			var team = TeamService.Get(player.TeamId);
			if(team == null || team.Type == TeamType.GANG || team.Type == TeamType.MAFIA) return;

			var account = AccountService.Get(player.DbId);
			if(account == null) return;

			account.TeamDuty = !account.TeamDuty;
			AccountService.Update(account);

			player.TeamDuty = account.TeamDuty;

			if (!account.TeamDuty)
			{
				player.SetArmor(0);

				var clothes = ClothesService.Get(player.ClothesId);
				if(clothes != null)
				{
					clothes.Armor = 0;
					clothes.ArmorColor = 0;
					clothes.ArmorDlc = 0;
					ClothesService.Update(clothes);
				}

				LoadoutService.ClearPlayerLoadout(player.DbId);
				player.RemoveAllWeapons(true);
				player.Weapons.Clear();
			}

			player.Notify("Fraktion", $"Du hast den Dienst {(account.TeamDuty ? "angetreten" : "verlassen")}.", NotificationType.INFO);
			TeamController.Broadcast(team.Id, $"{player.Name}(R{account.TeamRank}) hat den Dienst {(account.TeamDuty ? "angetreten" : "verlassen")}.", NotificationType.INFO);
		}

		private static void DepositMoney(RPPlayer player, int amount)
		{
			if (!player.LoggedIn || player.TeamId < 1) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || account.Money < amount) return;

			var team = TeamService.Get(player.TeamId);
			if (team == null) return;

			PlayerController.RemoveMoney(player, amount);

			team.Money += amount;
			TeamService.Update(team);

			var history = new BankHistoryModel(team.Id, player.Name, $"Fraktionsbank {team.ShortName}", TransactionType.TEAM, false, amount, DateTime.Now);
			BankService.AddHistory(history);

			player.Notify("Fraktion", $"Du hast ${amount} eingezahlt.", NotificationType.SUCCESS);
		}

		private static void WithdrawMoney(RPPlayer player, int amount)
		{
			if (!player.LoggedIn || player.TeamId < 1) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || !account.TeamBank) return;

			var team = TeamService.Get(player.TeamId);
			if (team == null || team.Money < amount) return;

			team.Money -= amount;
			TeamService.Update(team);

			PlayerController.AddMoney(player, amount);

			var history = new BankHistoryModel(team.Id, player.Name, $"Fraktionsbank {team.ShortName}", TransactionType.TEAM, true, amount, DateTime.Now);
			BankService.AddHistory(history);

			player.Notify("Fraktion", $"Du hast ${amount} ausgezahlt.", NotificationType.SUCCESS);
		}

		private static void KickMember(RPPlayer player, int id, bool mobile = false)
		{
			if (!player.LoggedIn || player.TeamId < 1 || player.DbId == id) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || !account.TeamAdmin) return;

			var target = AccountService.Get(id);
			if (target == null || target.TeamRank >= account.TeamRank) return;

			var federal = target.TeamId > 0 && target.TeamId <= 5;

			TeamController.Broadcast(account.TeamId, $"{target.Name} wurde von {player.Name} aus der Fraktion entlassen!", NotificationType.INFO);

			var targetPlayer = RPPlayer.All.FirstOrDefault(x => x.DbId == id);
			if (targetPlayer == null)
			{
				target.TeamId = 0;
				target.TeamRank = 0;
				target.TeamAdmin = false;
				target.TeamStorage = false;
				target.TeamBank = false;
				target.TeamDuty = false;
				target.SWATDuty = false;
				AccountService.Update(target);
				return;
			}

			if(federal)
			{
				targetPlayer.RemoveAllWeapons(true);
				targetPlayer.Weapons.Clear();
				LoadoutService.ClearPlayerLoadout(target.Id);
			}

			targetPlayer.TeamDuty = false;
			targetPlayer.SWATDuty = false;
			targetPlayer.TeamId = 0;
			TeamController.SetPlayerTeam(targetPlayer, 0, 0, false, false, false);
			targetPlayer.Notify("Fraktion", $"Du wurdest von {player.Name} aus der Fraktion entlassen.", NotificationType.INFO);
		}

		private static void BuyEquip(RPPlayer player)
		{
			if (!player.LoggedIn || player.TeamId < 1) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var team = TeamService.Get(player.TeamId);
			if (team == null) return;

			var weapons = new List<LoadoutModel>()
			{
				new(player.DbId, 3219281620, 500),
				new(player.DbId, team.MeeleWeaponHash, 0)
			};

			if(team.Type == TeamType.FEDERAL)
			{
				weapons.Add(new(player.DbId, 911657153u, 0));
				weapons.Add(new(player.DbId, 1233104067u, 25));
			}
			else
			{
				if (account.Money < 2500)
				{
					player.Notify("Fraktion", "Du benötigst $2500!", NotificationType.ERROR);
					return;
				}

				account.Money -= 2500;
				AccountService.Update(account);
			}
			
			LoadoutService.AddRange(weapons);
			PlayerController.ApplyPlayerLoadout(player);
			player.Notify("Fraktion", "Du hast dich ausgerüstet!", NotificationType.SUCCESS);
		}

		private static void UpdateMember(RPPlayer player, int targetId, int rank, bool leader, bool storage, bool bank)
		{
			if (!player.LoggedIn || player.TeamId < 1 || player.DbId == targetId) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || !account.TeamAdmin) return;

			var target = AccountService.Get(targetId);
			if (target == null) return;

			TeamController.Broadcast(account.TeamId, $"{player.Name} hat {target.Name} auf Rang {rank} gesetzt!", NotificationType.INFO);

			var targetPlayer = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (targetPlayer == null)
			{
				target.TeamRank = rank;
				target.TeamAdmin = leader;
				target.TeamStorage = storage;
				target.TeamBank = bank;
				AccountService.Update(target);
				return;
			}

			TeamController.SetPlayerTeam(targetPlayer, targetPlayer.TeamId, rank, leader, storage, bank);
		}

		private static void OpenTeamMenu(RPPlayer player)
		{
			if (!player.LoggedIn || player.TeamId < 1) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var team = TeamService.Get(player.TeamId);
			if (team == null) return;

			var pos = PositionService.Get(team.PositionId);
			if (pos == null || player.Position.Distance(pos.Position) > 2f) return;

			if (team.Type == TeamType.GANG || team.Type == TeamType.MAFIA) OpenCrimeTeamMenu(player, account, team);
			else OpenFederalTeamMenu(player, account, team);
		}

		private static void OpenFederalTeamMenu(RPPlayer player, AccountModel account, TeamModel team)
		{
			var members = new List<TeamClientMemberData>();

			foreach(var member in AccountService.GetFromTeam(team.Id))
			{
				members.Add(new(
					member.Id,
					member.Name,
					member.TeamRank,
					RPPlayer.All.FirstOrDefault(x => x.DbId == member.Id) != null,
					member.TeamAdmin,
					member.TeamStorage,
					member.TeamBank,
					member.LastOnline.ToString(),
					member.TeamJoinDate.ToString()));
			}

			var teamData = new ClientFederalTeam(
				team.Id,
				team.Name,
				team.Warns,
				PoliceModule.SWATStatus,
				team.Money,
				members,
				GetBankHistory(team.Id),
				new(0, 0, 0));

			player.ShowComponent("Team", true, JsonConvert.SerializeObject(new TeamFederalClientData(
				player.Name,
				team.MeeleWeapon,
				account.TeamAdmin,
				account.TeamBank,
				account.TeamDuty,
				account.TeamStorage,
				teamData)));
		}

		private static void OpenCrimeTeamMenu(RPPlayer player, AccountModel account, TeamModel team)
		{
			var lab = TeamService.GetLaboratoryByTeam(team.Id);
			if (lab == null) return;

			var labPos = PositionService.Get(lab.PositionId);
			if (labPos == null) return;


			var inventories = WarehouseService.GetWarehouseInventories(team.Id, OwnerType.TEAM);
			var weight = 0f;
			var maxWeight = 0f;
			var slots = 0;
			var maxSlots = 0;
			var drugs = 0;

			foreach(var inventory in inventories)
			{
				var items = InventoryService.GetInventoryItems(inventory.Id);

				maxWeight += inventory.MaxWeight;
				slots += items.Count;
				maxSlots += inventory.Slots;

				foreach(var item in items)
				{
					var itemBase = InventoryService.GetItem(item.ItemId);
					if (itemBase == null) continue;

					weight += itemBase.Weight * item.Amount;
					if(item.ItemId == 3) drugs += item.Amount;
				}
			}

			var storageData = new TeamStorageClientData(
				3,
				(int)weight,
				(int)maxWeight,
				slots,
				maxSlots,
				drugs);

			var members = new List<TeamClientMemberData>();

			foreach (var member in AccountService.GetFromTeam(team.Id))
			{
				members.Add(new(
					member.Id,
					member.Name,
					member.TeamRank,
					RPPlayer.All.FirstOrDefault(x => x.DbId == member.Id) != null,
					member.TeamAdmin,
					member.TeamStorage,
					member.TeamBank,
					$"{member.LastOnline.Day}.{member.LastOnline.Month}.{member.LastOnline.Year}",
					$"{account.TeamJoinDate.Day}.{account.TeamJoinDate.Month}.{account.TeamJoinDate.Year}"));
			}

			var gws = GangwarService.GetFromTeam(team.Id);

			var runningGangwar = GangwarController.RunningGangwars.FirstOrDefault(x => x.OwnerId == team.Id || x.AttackerId == team.Id);

			TeamClientGangwarData? gwData = null;

			if(runningGangwar != null)
			{
				gwData = new(runningGangwar.Name, runningGangwar.OwnerId == team.Id ? runningGangwar.AttackerName : runningGangwar.OwnerName);
			}

			var teamData = new ClientTeam(
				team.Id,
				team.Name,
				team.Warns,
				gws.Count,
				team.Money,
				storageData,
				members,
				GetBankHistory(team.Id),
				new(labPos.Position, TeamController.GetLaboratoryFuel(team.Id)),
				new(0, 0, 0));

			player.ShowComponent("Team", true, JsonConvert.SerializeObject(new TeamClientData(
				player.Name,
				team.MeeleWeapon,
				account.TeamAdmin,
				account.TeamBank,
				teamData,
				gwData)));
		}

		private static List<object> GetBankHistory(int teamId)
		{
			var history = BankService.GetHistory(teamId, TransactionType.TEAM, 30);
			var result = new List<object>();

			foreach (var item in history)
			{
				result.Add(new
				{
					Name = item.Name,
					Amount = item.Amount,
					Date = item.Date.ToLocalTime(),
					Type = item.Withdraw
				});
			}

			return result;
		}
	}
}