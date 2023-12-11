using Database.Models.Bank;
using Database.Models.DPOS;

namespace Database.Services
{
	public static class ImpoundService
	{
		public static List<ImpoundModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Impounds.ToList();
		}

		public static void Add(ImpoundModel model)
		{
			using var ctx = new Context();
			ctx.Impounds.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(ImpoundModel model)
		{
			using var ctx = new Context();
			ctx.Impounds.Remove(model);
			ctx.SaveChanges();
		}

		public static ImpoundModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Impounds.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(ImpoundModel model)
		{
			using var ctx = new Context();
			ctx.Impounds.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<ImpoundModel> models)
		{
			using var ctx = new Context();
			ctx.Impounds.UpdateRange(models);
			ctx.SaveChanges();
		}

		// spawn
		public static List<ImpoundSpawnModel> GetSpawns(int impoundId)
		{
			using var ctx = new Context();
			return ctx.ImpoundSpawns.Where(x => x.ImpoundId == impoundId).ToList();
		}

		public static void AddSpawn(ImpoundSpawnModel model)
		{
			using var ctx = new Context();
			ctx.ImpoundSpawns.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveSpawn(ImpoundSpawnModel model)
		{
			using var ctx = new Context();
			ctx.ImpoundSpawns.Remove(model);
			ctx.SaveChanges();
		}

		public static ImpoundSpawnModel? GetSpawn(int id)
		{
			using var ctx = new Context();
			return ctx.ImpoundSpawns.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateSpawn(ImpoundSpawnModel model)
		{
			using var ctx = new Context();
			ctx.ImpoundSpawns.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateSpawns(IEnumerable<ImpoundSpawnModel> models)
		{
			using var ctx = new Context();
			ctx.ImpoundSpawns.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}