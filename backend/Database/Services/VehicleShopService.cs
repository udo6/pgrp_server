using Database.Models.VehicleShop;

namespace Database.Services
{
    public static class VehicleShopService
	{
		public static List<VehicleShopModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.VehicleShops.ToList();
		}

		public static void Add(VehicleShopModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShops.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(VehicleShopModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShops.Remove(model);
			ctx.SaveChanges();
		}

		public static VehicleShopModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.VehicleShops.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(VehicleShopModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShops.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<VehicleShopModel> models)
		{
			using var ctx = new Context();
			ctx.VehicleShops.UpdateRange(models);
			ctx.SaveChanges();
		}

		// items
		public static List<VehicleShopItemModel> GetItemsFromShop(int shopId)
		{
			using var ctx = new Context();
			return ctx.VehicleShopItems.Where(x => x.ShopId == shopId).ToList();
		}

		public static void AddItem(VehicleShopItemModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShopItems.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveItem(VehicleShopItemModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShopItems.Remove(model);
			ctx.SaveChanges();
		}

		public static VehicleShopItemModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.VehicleShopItems.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateItem(VehicleShopItemModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShopItems.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateItems(IEnumerable<VehicleShopItemModel> models)
		{
			using var ctx = new Context();
			ctx.VehicleShopItems.UpdateRange(models);
			ctx.SaveChanges();
		}

		// spawns
		public static List<VehicleShopSpawnModel> GetSpawnsFromShop(int shopId)
		{
			using var ctx = new Context();
			return ctx.VehicleShopSpawns.Where(x => x.ShopId == shopId).ToList();
		}

		public static void AddSpawn(VehicleShopSpawnModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShopSpawns.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveSpawn(VehicleShopSpawnModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShopSpawns.Remove(model);
			ctx.SaveChanges();
		}

		public static VehicleShopSpawnModel? GetSpawn(int id)
		{
			using var ctx = new Context();
			return ctx.VehicleShopSpawns.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateSpawn(VehicleShopSpawnModel model)
		{
			using var ctx = new Context();
			ctx.VehicleShopSpawns.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateSpawns(IEnumerable<VehicleShopSpawnModel> models)
		{
			using var ctx = new Context();
			ctx.VehicleShopSpawns.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}