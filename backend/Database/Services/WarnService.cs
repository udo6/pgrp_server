using Database.Models.Account;
using Database.Models.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services
{
	public static class WarnService
	{
		public static int GetPlayerWarnsCount(int accountId)
		{
			using var ctx = new Context();
			return ctx.Warns.Count(x => x.AccountId == accountId);
		}

		public static List<WarnModel> GetPlayerWarns(int accountId)
		{
			using var ctx = new Context();
			return ctx.Warns.Where(x => x.AccountId == accountId).ToList();
		}

		public static void Add(WarnModel model)
		{
			using var ctx = new Context();
			ctx.Warns.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(WarnModel model)
		{
			using var ctx = new Context();
			ctx.Warns.Remove(model);
			ctx.SaveChanges();
		}

		public static WarnModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Warns.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(WarnModel model)
		{
			using var ctx = new Context();
			ctx.Warns.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<WarnModel> models)
		{
			using var ctx = new Context();
			ctx.Warns.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}
