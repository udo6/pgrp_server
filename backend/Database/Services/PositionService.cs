using Database.Models;

namespace Database.Services
{
	public static class PositionService
	{
		public static void Add(PositionModel model)
		{
			using var ctx = new Context();
			ctx.Positions.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(PositionModel model)
		{
			using var ctx = new Context();
			ctx.Positions.Remove(model);
			ctx.SaveChanges();
		}

		public static PositionModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Positions.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(PositionModel model)
		{
			using var ctx = new Context();
			ctx.Positions.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<PositionModel> models)
		{
			using var ctx = new Context();
			ctx.Positions.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}