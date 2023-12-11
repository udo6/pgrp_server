﻿using Logs.Enums;
using Logs.Models;
using Logs.Models.Inventory;

namespace Logs
{
	public static class LogService
	{
		public static void LogInventoryMove(int inventoryId, int containerId, int itemId, int amount, InventoryMoveType type)
		{
			var model = new InventoryMoveModel(inventoryId, containerId, itemId, amount, type);

			using var ctx = new Context();
			ctx.InventoryMoveLogs.Add(model);
			ctx.SaveChanges();
		}

		public static void LogMoneyTransaction(int accountId, int targetId, int amount, MoneyTransactionType type)
		{
			var model = new MoneyTransactionModel(accountId, targetId, amount, type);

			using var ctx = new Context();
			ctx.MoneyTransactionLogs.Add(model);
			ctx.SaveChanges();
		}

		public static void LogPlayerConnection(int accountId, bool join)
		{
			var model = new PlayerConnectionModel(accountId, join);

			using var ctx = new Context();
			ctx.PlayerConnectionLogs.Add(model);
			ctx.SaveChanges();
		}

		public static void LogPlayerBan(int accountId, int adminId, string reason)
		{
			var model = new BanModel(accountId, adminId, reason);

			using var ctx = new Context();
			ctx.BanLogs.Add(model);
			ctx.SaveChanges();
		}
	}
}