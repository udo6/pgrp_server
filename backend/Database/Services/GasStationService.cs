using Database.Models.GasStation;

namespace Database.Services
{
    public static class GasStationService
	{
		public static List<GasStationModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.GasStations.ToList();
		}

		public static void Add(GasStationModel model)
		{
			using var ctx = new Context();
			ctx.GasStations.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(GasStationModel model)
		{
			using var ctx = new Context();
			ctx.GasStations.Remove(model);
			ctx.SaveChanges();
		}

		public static GasStationModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.GasStations.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(GasStationModel model)
		{
			using var ctx = new Context();
			ctx.GasStations.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<GasStationModel> models)
		{
			using var ctx = new Context();
			ctx.GasStations.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}