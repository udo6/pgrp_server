using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Bank;
using Database.Services;
using Game.Controllers;
using Logs;
using Newtonsoft.Json;

namespace Game.Modules
{
	public static class BankModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var bank in BankService.GetAll())
				BankController.LoadBank(bank);

			Alt.OnClient<RPPlayer, int>("Server:Bank:Open", Open);
			Alt.OnClient<RPPlayer, int, int>("Server:Bank:Deposit", Deposit);
			Alt.OnClient<RPPlayer, int>("Server:Bank:DepositAll", DepositAll);
			Alt.OnClient<RPPlayer, int, int>("Server:Bank:Withdraw", Withdraw);
		}

		private static void Open(RPPlayer player, int id)
		{
			if (!player.LoggedIn) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var bank = BankService.Get(id);
			if (bank == null) return;

			var history = BankService.GetHistory(player.DbId, TransactionType.PLAYER, 15);

			player.ShowComponent("Bank", true, JsonConvert.SerializeObject(new
			{
				Id = bank.Id,
				Name = bank.Name,
				Balance = account.BankMoney,
				History = history
			}));
		}

		private static void Deposit(RPPlayer player, int bankId, int amount)
		{
			if (!player.LoggedIn || amount < 1) return;

			var bank = BankService.Get(bankId);
			if (bank == null) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(account.Money < amount)
			{
				player.ShowComponent("Bank", false);
				player.Notify("Information", "Du hast nicht so viel Geld bei dir.", NotificationType.ERROR);
				return;
			}

			account.Money -= amount;
			account.BankMoney += amount;
			AccountService.Update(account);

			player.EmitBrowser("Hud:SetMoney", account.Money);

			var history = new BankHistoryModel(player.DbId, player.Name, bank.Name, TransactionType.PLAYER, false, amount, DateTime.Now);
			BankService.AddHistory(history);
			player.Notify("Information", $"Du hast ${amount} auf die Bank eingezahlt!", NotificationType.SUCCESS);

			LogService.LogMoneyTransaction(player.DbId, player.DbId, amount, Logs.Enums.MoneyTransactionType.PLAYER_TO_BANK);
		}

		private static void DepositAll(RPPlayer player, int bankId)
		{
			if (!player.LoggedIn) return;

			var bank = BankService.Get(bankId);
			if (bank == null) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var amount = account.Money;

			account.BankMoney += amount;
			account.Money = 0;
			AccountService.Update(account);

			player.EmitBrowser("Hud:SetMoney", 0);

			var history = new BankHistoryModel(player.DbId, player.Name, bank.Name, TransactionType.PLAYER, false, amount, DateTime.Now);
			BankService.AddHistory(history);
			player.Notify("Information", $"Du hast ${amount} auf die Bank eingezahlt!", NotificationType.SUCCESS);

			LogService.LogMoneyTransaction(player.DbId, player.DbId, amount, Logs.Enums.MoneyTransactionType.PLAYER_TO_BANK);
		}

		private static void Withdraw(RPPlayer player, int bankId, int amount)
		{
			if (!player.LoggedIn || amount < 1) return;

			var bank = BankService.Get(bankId);
			if (bank == null) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if (account.BankMoney < amount)
			{
				player.ShowComponent("Bank", false);
				player.Notify("Information", "Du hast nicht so viel Geld auf der Bank.", NotificationType.ERROR);
				return;
			}

			account.Money += amount;
			account.BankMoney -= amount;
			AccountService.Update(account);

			player.EmitBrowser("Hud:SetMoney", account.Money);

			var history = new BankHistoryModel(player.DbId, player.Name, bank.Name, TransactionType.PLAYER, true, amount, DateTime.Now);
			BankService.AddHistory(history);
			player.Notify("Information", $"Du hast ${amount} von der Bank abgehoben!", NotificationType.SUCCESS);

			LogService.LogMoneyTransaction(player.DbId, player.DbId, amount, Logs.Enums.MoneyTransactionType.BANK_TO_PLAYER);
		}
	}
}