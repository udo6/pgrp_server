using Database.Models.Vehicle;

namespace Database.Services
{
    public static class TuningService
	{
		public static TuningModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Tunings.FirstOrDefault(x => x.Id == id);
		}

		public static void Add(TuningModel model)
		{
			using var ctx = new Context();
			ctx.Tunings.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(TuningModel model)
		{
			using var ctx = new Context();
			ctx.Tunings.Remove(model);
			ctx.SaveChanges();
		}

		public static void Update(TuningModel model)
		{
			using var ctx = new Context();
			ctx.Tunings.Update(model);
			ctx.SaveChanges();
		}
	}
}