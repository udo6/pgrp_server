using Database.Models.Account;
using Database.Models.Dealer;

namespace Database.Services
{
	public static class DealerService
	{
		public static List<DealerModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Dealers.ToList();
		}

		public static void Add(DealerModel model)
		{
			using var ctx = new Context();
			ctx.Dealers.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(DealerModel model)
		{
			using var ctx = new Context();
			ctx.Dealers.Remove(model);
			ctx.SaveChanges();
		}

		public static DealerModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Dealers.FirstOrDefault(x => x.Id == id);
		}

		// items
		public static List<DealerItemModel> GetAllItems()
		{
			using var ctx = new Context();
			return ctx.DealerItems.ToList();
		}

		public static void AddItem(DealerItemModel model)
		{
			using var ctx = new Context();
			ctx.DealerItems.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveItem(DealerItemModel model)
		{
			using var ctx = new Context();
			ctx.DealerItems.Remove(model);
			ctx.SaveChanges();
		}

		public static DealerItemModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.DealerItems.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateItem(DealerItemModel model)
		{
			using var ctx = new Context();
			ctx.DealerItems.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateItems(IEnumerable<DealerItemModel> models)
		{
			using var ctx = new Context();
			ctx.DealerItems.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}
