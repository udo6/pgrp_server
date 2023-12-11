using Database.Models;
using Database.Models.Account;

namespace Database.Services
{
	public static class ExportDealerService
	{
		public static List<ExportDealerItemModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.ExportDealerItems.ToList();
		}

		public static void Add(ExportDealerItemModel model)
		{
			using var ctx = new Context();
			ctx.ExportDealerItems.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(ExportDealerItemModel model)
		{
			using var ctx = new Context();
			ctx.ExportDealerItems.Remove(model);
			ctx.SaveChanges();
		}

		public static ExportDealerItemModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.ExportDealerItems.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(ExportDealerItemModel model)
		{
			using var ctx = new Context();
			ctx.ExportDealerItems.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<ExportDealerItemModel> models)
		{
			using var ctx = new Context();
			ctx.ExportDealerItems.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}