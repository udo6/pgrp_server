using Database.Models.Account;

namespace Database.Services
{
    public static class LicenseService
	{
		public static void Add(LicenseModel model)
		{
			using var ctx = new Context();
			ctx.Licenses.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(LicenseModel model)
		{
			using var ctx = new Context();
			ctx.Licenses.Remove(model);
			ctx.SaveChanges();
		}

		public static LicenseModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Licenses.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(LicenseModel model)
		{
			using var ctx = new Context();
			ctx.Licenses.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<LicenseModel> models)
		{
			using var ctx = new Context();
			ctx.Licenses.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}