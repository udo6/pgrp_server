using Core.Enums;
using Database.Models.Bank;

namespace Database.Services
{
    public static class BankService
	{
		public static List<BankModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Banks.ToList();
		}

		public static void Add(BankModel model)
		{
			using var ctx = new Context();
			ctx.Banks.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(BankModel model)
		{
			using var ctx = new Context();
			ctx.Banks.Remove(model);
			ctx.SaveChanges();
		}

		public static BankModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Banks.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(BankModel model)
		{
			using var ctx = new Context();
			ctx.Banks.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<BankModel> models)
		{
			using var ctx = new Context();
			ctx.Banks.UpdateRange(models);
			ctx.SaveChanges();
		}

		public static List<BankHistoryModel> GetHistory(int account, TransactionType type, int max)
		{
			using var ctx = new Context();
			return ctx.BankHistory.Where(x => x.AccountId == account && x.Type == type).OrderByDescending(x => x.Date).Take(max).ToList();
		}

		public static void AddHistory(BankHistoryModel model)
		{
			using var ctx = new Context();
			ctx.BankHistory.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveHistory(BankHistoryModel model)
		{
			using var ctx = new Context();
			ctx.BankHistory.Remove(model);
			ctx.SaveChanges();
		}
	}
}