﻿using Logs.Enums;
using Logs.Models;
using Logs.Models.Inventory;

namespace Logs
{
	public static class LogService
	{
		private static List<DamageModel> DamageLogsCache = new();
		private static List<MagicBulletModel> MagicBulletLogsCache = new();

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

		public static void LogExplosion(int playerId, int explosionType)
		{
			var model = new ExplosionModel(playerId, explosionType, DateTime.Now);

			using var ctx = new Context();
			ctx.Explosions.Add(model);
			ctx.SaveChanges();
		}

		public static void LogACPAction(int accountId, int targetId, TargetType targetType, ACPActionType actionType)
		{
			var model = new ACPActionModel(accountId, targetId, targetType, actionType, DateTime.Now);

			using var ctx = new Context();
			ctx.ACPActions.Add(model);
			ctx.SaveChanges();
		}

		public static List<ACPActionModel> GetACPLogs(int adminId, int targetId, int take)
		{
			using var ctx = new Context();
			return ctx.ACPActions.Where(x => (adminId == 0 || x.AccountId == adminId) && (targetId == 0 || x.TargetId == targetId)).OrderByDescending(x => x.Date).Take(take).ToList();
		}

		public static void LogKill(int accountId, int killerId, uint weapon)
		{
			var model = new KillModel(accountId, killerId, weapon, DateTime.Now);

			using var ctx = new Context();
			ctx.KillLogs.Add(model);
			ctx.SaveChanges();
		}

		public static List<KillModel> GetKillLogs(int accountId, int killerId, DateTime datetime)
		{
			using var ctx = new Context();
			return ctx.KillLogs.Where(x => (accountId == 0 || x.AccountId == accountId) && (killerId == 0 || x.KillerId == killerId) && x.Date.Year == datetime.Year && x.Date.Month == datetime.Month && x.Date.Day == datetime.Day && x.Date.Hour > datetime.Hour - 1 && x.Date.Hour < datetime.Hour + 1).OrderByDescending(x => x.Date).ToList();
		}

		public static void LogDamage(int accountId, int targetId, uint weapon, int damage, int bodyPart)
		{
			DamageLogsCache.Add(new(accountId, targetId, weapon, damage, bodyPart, DateTime.Now));
		}

		public static List<DamageModel> GetDamageLogs(int attackerId, int targetId, DateTime datetime)
		{
			using var ctx = new Context();
			return ctx.DamageLogs.Where(x => (attackerId == 0 || x.AccountId == attackerId) && (targetId == 0 || x.TargetId == targetId) && x.Date.Year == datetime.Year && x.Date.Month == datetime.Month && x.Date.Day == datetime.Day && x.Date.Hour > datetime.Hour - 1 && x.Date.Hour < datetime.Hour + 1).OrderByDescending(x => x.Date).ToList();
		}

		public static void LogMagicBullet(int accountId, uint weapon, int damage, float distance)
		{
			MagicBulletLogsCache.Add(new(accountId, weapon, damage, distance, DateTime.Now));
		}

		public static void SendDamageLogsToDatabase()
		{
			using var ctx = new Context();
			ctx.DamageLogs.AddRange(DamageLogsCache);
			ctx.SaveChanges();

			DamageLogsCache.Clear();
		}

		public static void SendMagicBulletLogsToDatabase()
		{
			using var ctx = new Context();
			ctx.MagicBulletLogs.AddRange(MagicBulletLogsCache);
			ctx.SaveChanges();

			MagicBulletLogsCache.Clear();
		}
	}
}