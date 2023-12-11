using Database.Models.Account;

namespace Database.Services
{
    public static class CustomizationService
	{
		public static void Add(CustomizationModel model)
		{
			using var ctx = new Context();
			ctx.Customizations.Add(model);
			ctx.SaveChanges();
		}

		public static CustomizationModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Customizations.FirstOrDefault(x => x.Id == id);
		}

		public static void Remove(CustomizationModel model)
		{
			using var ctx = new Context();
			ctx.Customizations.Remove(model);
			ctx.SaveChanges();
		}

		public static void Update(CustomizationModel model)
		{
			using var ctx = new Context();
			ctx.Customizations.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<CustomizationModel> models)
		{
			using var ctx = new Context();
			ctx.Customizations.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}