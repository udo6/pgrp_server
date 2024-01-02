using Core.Enums;
using Database.Models.ClothesShop;

namespace Database.Services
{
    public static class ClothesShopService
	{
		public static void Add(ClothesShopModel model)
		{
			using var ctx = new Context();
			ctx.ClothesShops.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(ClothesShopModel model)
		{
			using var ctx = new Context();
			ctx.ClothesShops.Remove(model);
			ctx.SaveChanges();
		}

		public static List<ClothesShopModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.ClothesShops.ToList();
		}

		public static ClothesShopModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.ClothesShops.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(ClothesShopModel model)
		{
			using var ctx = new Context();
			ctx.ClothesShops.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<ClothesShopModel> models)
		{
			using var ctx = new Context();
			ctx.ClothesShops.UpdateRange(models);
			ctx.SaveChanges();
		}

		// Items
		public static void AddItem(ClothesShopItemModel model)
		{
			using var ctx = new Context();
			ctx.ClothesShopItems.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveItem(ClothesShopItemModel model)
		{
			using var ctx = new Context();
			ctx.ClothesShopItems.Remove(model);
			ctx.SaveChanges();
		}

		public static List<ClothesShopItemModel> GetItemsFromShop(ClothesShopType type, int gender)
		{
			using var ctx = new Context();
			return ctx.ClothesShopItems.Where(x => (x.ShopId == (int)type || x.ShopId == 0) && (x.Gender == 2 || x.Gender == gender)).ToList();
		}

		public static ClothesShopItemModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.ClothesShopItems.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateItem(ClothesShopItemModel model)
		{
			using var ctx = new Context();
			ctx.ClothesShopItems.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateItems(IEnumerable<ClothesShopItemModel> models)
		{
			using var ctx = new Context();
			ctx.ClothesShopItems.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}