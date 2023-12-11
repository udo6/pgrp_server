using Database.Models.Shop;

namespace Database.Services
{
    public static class ShopService
	{
		public static List<ShopModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Shops.ToList();
		}

		public static void Add(ShopModel model)
		{
			using var ctx = new Context();
			ctx.Shops.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(ShopModel model)
		{
			using var ctx = new Context();
			ctx.Shops.Remove(model);
			ctx.SaveChanges();
		}

		public static ShopModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Shops.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(ShopModel model)
		{
			using var ctx = new Context();
			ctx.Shops.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<ShopModel> models)
		{
			using var ctx = new Context();
			ctx.Shops.UpdateRange(models);
			ctx.SaveChanges();
		}

		public static List<ShopItemModel> GetShopItems(int shopId)
		{
			using var ctx = new Context();
			return ctx.ShopItems.Where(x => x.ShopId == shopId).ToList();
		}

		public static List<ShopItemModel> GetAllItems()
		{
			using var ctx = new Context();
			return ctx.ShopItems.ToList();
		}

		public static void AddItem(ShopItemModel model)
		{
			using var ctx = new Context();
			ctx.ShopItems.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveItem(ShopItemModel model)
		{
			using var ctx = new Context();
			ctx.ShopItems.Remove(model);
			ctx.SaveChanges();
		}

		public static ShopItemModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.ShopItems.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateItem(ShopItemModel model)
		{
			using var ctx = new Context();
			ctx.ShopItems.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateItems(IEnumerable<ShopItemModel> models)
		{
			using var ctx = new Context();
			ctx.ShopItems.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}