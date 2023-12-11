using Database.Models.Account;
using Database.Models.Tattoo;

namespace Database.Services
{
	public static class TattooService
	{
		public static List<TattooModel> GetFromAccount(int accountId)
		{
			using var ctx = new Context();
			return ctx.Tattoos.Where(x => x.AccountId == accountId).ToList();
		}

		public static bool HasItem(int accountId, uint collection, uint overlay)
		{
			using var ctx = new Context();
			return ctx.Tattoos.Any(x => x.AccountId == accountId && x.Collection == collection && x.Overlay == overlay);
		}

		public static void Add(TattooModel model)
		{
			using var ctx = new Context();
			ctx.Tattoos.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(TattooModel model)
		{
			using var ctx = new Context();
			ctx.Tattoos.Remove(model);
			ctx.SaveChanges();
		}

		// shops
		public static List<TattooShopModel> GetAllShops()
		{
			using var ctx = new Context();
			return ctx.TattooShops.ToList();
		}

		public static void AddShop(TattooShopModel model)
		{
			using var ctx = new Context();
			ctx.TattooShops.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveShop(TattooShopModel model)
		{
			using var ctx = new Context();
			ctx.TattooShops.Remove(model);
			ctx.SaveChanges();
		}

		// items
		public static List<TattooShopItemModel> GetItemsFromShop(int shopId)
		{
			using var ctx = new Context();
			return ctx.TattooShopItems.Where(x => x.TattooShopId == shopId).ToList();
		}

		public static TattooShopItemModel? GetItem(int itemId)
		{
			using var ctx = new Context();
			return ctx.TattooShopItems.FirstOrDefault(x => x.Id == itemId);
		}

		public static void AddItem(TattooShopItemModel model)
		{
			using var ctx = new Context();
			ctx.TattooShopItems.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveItem(TattooShopItemModel model)
		{
			using var ctx = new Context();
			ctx.TattooShopItems.Remove(model);
			ctx.SaveChanges();
		}
	}
}