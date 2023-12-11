using Database.Models.Account;

namespace Database.Services
{
    public static class ClothesService
	{
		public static void Add(ClothesModel model)
		{
			using var ctx = new Context();
			ctx.Clothes.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(ClothesModel model)
		{
			using var ctx = new Context();
			ctx.Clothes.Remove(model);
			ctx.SaveChanges();
		}

		public static ClothesModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Clothes.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(ClothesModel model)
		{
			using var ctx = new Context();
			ctx.Clothes.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<ClothesModel> models)
		{
			using var ctx = new Context();
			ctx.Clothes.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}