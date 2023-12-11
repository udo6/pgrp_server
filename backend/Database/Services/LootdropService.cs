using Database.Models.Lootdrop;

namespace Database.Services
{
    public static class LootdropService
	{
		public static List<LootdropModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Lootdrops.ToList();
		}

		public static void Add(LootdropModel model)
		{
			using var ctx = new Context();
			ctx.Lootdrops.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(LootdropModel model)
		{
			using var ctx = new Context();
			ctx.Lootdrops.Remove(model);
			ctx.SaveChanges();
		}

		public static LootdropModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Lootdrops.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(LootdropModel model)
		{
			using var ctx = new Context();
			ctx.Lootdrops.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<LootdropModel> models)
		{
			using var ctx = new Context();
			ctx.Lootdrops.UpdateRange(models);
			ctx.SaveChanges();
		}

		// items
		public static List<LootdropLootModel> GetAllItems()
		{
			using var ctx = new Context();
			return ctx.LootdropLoot.ToList();
		}

		public static void AddItem(LootdropLootModel model)
		{
			using var ctx = new Context();
			ctx.LootdropLoot.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveItem(LootdropLootModel model)
		{
			using var ctx = new Context();
			ctx.LootdropLoot.Remove(model);
			ctx.SaveChanges();
		}

		public static LootdropLootModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.LootdropLoot.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateItem(LootdropLootModel model)
		{
			using var ctx = new Context();
			ctx.LootdropLoot.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateItems(IEnumerable<LootdropLootModel> models)
		{
			using var ctx = new Context();
			ctx.LootdropLoot.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}