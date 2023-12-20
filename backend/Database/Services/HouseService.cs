using Database.Models.House;

namespace Database.Services
{
    public static class HouseService
	{
		public static List<HouseModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Houses.ToList();
		}

		public static void Add(HouseModel model)
		{
			using var ctx = new Context();
			ctx.Houses.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(HouseModel model)
		{
			using var ctx = new Context();
			ctx.Houses.Remove(model);
			ctx.SaveChanges();
		}

		public static HouseModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Houses.FirstOrDefault(x => x.Id == id);
		}

		public static bool HasPlayerHouse(int ownerId)
		{
			using var ctx = new Context();
			return ctx.Houses.Any(x => x.OwnerId == ownerId);
		}

		public static HouseModel? GetByOwner(int accountid)
		{
			using var ctx = new Context();
			return ctx.Houses.FirstOrDefault(x => x.OwnerId == accountid);
		}

		public static void Update(HouseModel model)
		{
			using var ctx = new Context();
			ctx.Houses.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<HouseModel> models)
		{
			using var ctx = new Context();
			ctx.Houses.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}