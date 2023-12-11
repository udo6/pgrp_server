using Database.Models.Farming;

namespace Database.Services
{
    public static class FarmingService
	{
		public static List<FarmingModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Farmings.ToList();
		}

		public static void Add(FarmingModel model)
		{
			using var ctx = new Context();
			ctx.Farmings.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(FarmingModel model)
		{
			using var ctx = new Context();
			ctx.Farmings.Remove(model);
			ctx.SaveChanges();
		}

		public static FarmingModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Farmings.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(FarmingModel model)
		{
			using var ctx = new Context();
			ctx.Farmings.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<FarmingModel> models)
		{
			using var ctx = new Context();
			ctx.Farmings.UpdateRange(models);
			ctx.SaveChanges();
		}

		// spots
		public static List<FarmingSpotModel> GetFromFarming(int farmingId)
		{
			using var ctx = new Context();
			return ctx.FarmingSpots.Where(x => x.FarmingId == farmingId).ToList();
		}

		public static void AddSpot(FarmingSpotModel model)
		{
			using var ctx = new Context();
			ctx.FarmingSpots.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveSpot(FarmingSpotModel model)
		{
			using var ctx = new Context();
			ctx.FarmingSpots.Remove(model);
			ctx.SaveChanges();
		}

		public static FarmingSpotModel? GetSpot(int id)
		{
			using var ctx = new Context();
			return ctx.FarmingSpots.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateSpot(FarmingSpotModel model)
		{
			using var ctx = new Context();
			ctx.FarmingSpots.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateSpot(IEnumerable<FarmingSpotModel> models)
		{
			using var ctx = new Context();
			ctx.FarmingSpots.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}