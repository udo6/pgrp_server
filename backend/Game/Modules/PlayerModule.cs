using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
using Game.Controllers;
using Logs;

namespace Game.Modules
{
	public static class PlayerModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:Player:StopInteraction", StopInteraction);
			Alt.OnClient<RPPlayer, int, string>("Server:Player.GiveMoney", GiveMoney);
		}

		private static void StopInteraction(RPPlayer player)
		{
			if (!player.Interaction) return;

			player.StopInteraction();
		}

		private static void GiveMoney(RPPlayer player, int targetId, string data)
		{
			if (!int.TryParse(data, out var money)) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || account.Money <  money) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			PlayerController.RemoveMoney(player, money);
			PlayerController.AddMoney(target, money);
			player.Notify("Information", $"Du hast jemandem ${money} zugesteckt!", Core.Enums.NotificationType.INFO);
			target.Notify("Information", $"Jemand hat dir ${money} zugesteckt!", Core.Enums.NotificationType.INFO);

			LogService.LogMoneyTransaction(player.DbId, target.DbId, money, Logs.Enums.MoneyTransactionType.PLAYER_TO_PLAYER);
		}

		[EveryMinute]
		public static void Tick()
		{
			var now = DateTime.Now;

			foreach(var player in RPPlayer.All.ToList())
			{
				if (!player.LoggedIn) continue;

				player.XpTicks++;

				if(player.XpTicks >= 60)
				{
					var account = AccountService.Get(player.DbId);
					if (account == null) continue;

					player.Xp++;
					player.XpTicks = 0;

					if(player.TeamId > 0 && player.TeamId <= 5)
					{
						var paycheck = 2000 + 100 * account.TeamRank;
						if (now.Hour >= 22 || now.Hour <= 6) paycheck += (int)Math.Round(paycheck * 0.1);

						account.BankMoney += paycheck;
						BankService.AddHistory(new(account.Id, account.Name, $"Gehalt", TransactionType.PLAYER, false, paycheck, DateTime.Now));
						player.Notify("Information", "Du hast einen Paycheck erhalten!", Core.Enums.NotificationType.INFO);
					}

					account.SocialBonusMoney += 100 * player.Level;
					AccountService.Update(account);
				}

				if(player.Xp >= player.Level * 4)
				{
					player.Level++;
					player.Xp = 0;
					player.Notify("Information", "Du bist ein Level aufgestiegen!", Core.Enums.NotificationType.SUCCESS);
				}
			}
		}
	}
}