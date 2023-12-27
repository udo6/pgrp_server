using Database.Models.Jumpoint;

namespace Database.Services
{
    public static class JumppointService
	{
		public static List<JumppointModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Jumppoints.ToList();
		}

		public static void LockAllJumppoints()
		{
			using var ctx = new Context();
			foreach (var jumppoint in ctx.Jumppoints) jumppoint.Locked = true;
			ctx.SaveChanges();
		}

		public static void Add(JumppointModel model)
		{
			using var ctx = new Context();
			ctx.Jumppoints.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(JumppointModel model)
		{
			using var ctx = new Context();
			ctx.Jumppoints.Remove(model);
			ctx.SaveChanges();
		}

		public static JumppointModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Jumppoints.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(JumppointModel model)
		{
			using var ctx = new Context();
			ctx.Jumppoints.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<JumppointModel> models)
		{
			using var ctx = new Context();
			ctx.Jumppoints.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}