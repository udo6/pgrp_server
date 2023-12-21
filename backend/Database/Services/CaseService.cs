using Database.Models.Bank;
using Database.Models.Case;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services
{
	public static class CaseService
	{
		public static void Add(CaseLootModel model)
		{
			using var ctx = new Context();
			ctx.CaseLootTable.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(CaseLootModel model)
		{
			using var ctx = new Context();
			ctx.CaseLootTable.Remove(model);
			ctx.SaveChanges();
		}

		public static CaseLootModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.CaseLootTable.FirstOrDefault(x => x.Id == id);
		}

		public static List<CaseLootModel> GetFromCase(int caseId)
		{
			using var ctx = new Context();
			return ctx.CaseLootTable.Where(x => x.CaseId == caseId).ToList();
		}
	}
}
