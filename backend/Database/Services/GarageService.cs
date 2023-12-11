using AltV.Net.Data;
using Core.Entities;
using Database.Models.Garage;

namespace Database.Services
{
    public static class GarageService
	{
		public static void Add(GarageModel model)
		{
			using var ctx = new Context();
			ctx.Garages.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(GarageModel model)
		{
			using var ctx = new Context();
			ctx.Garages.Remove(model);
			ctx.SaveChanges();
		}

		public static GarageModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Garages.FirstOrDefault(x => x.Id == id);
		}

		public static List<GarageModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Garages.ToList();
		}

		public static void Update(GarageModel model)
		{
			using var ctx = new Context();
			ctx.Garages.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<GarageModel> models)
		{
			using var ctx = new Context();
			ctx.Garages.UpdateRange(models);
			ctx.SaveChanges();
		}

		// spawns
		public static void AddSpawn(GarageSpawnModel model)
		{
			using var ctx = new Context();
			ctx.GarageSpawns.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveSpawn(GarageSpawnModel model)
		{
			using var ctx = new Context();
			ctx.GarageSpawns.Remove(model);
			ctx.SaveChanges();
		}

		public static GarageSpawnModel? GetSpawn(int id)
		{
			using var ctx = new Context();
			return ctx.GarageSpawns.FirstOrDefault(x => x.Id == id);
		}

		public static List<GarageSpawnModel> GetGarageSpawns(int garageId)
		{
			using var ctx = new Context();
			return ctx.GarageSpawns.Where(x => x.GarageId == garageId).ToList();
		}

		public static void UpdateSpawn(GarageSpawnModel model)
		{
			using var ctx = new Context();
			ctx.GarageSpawns.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateSpawns(IEnumerable<GarageSpawnModel> models)
		{
			using var ctx = new Context();
			ctx.GarageSpawns.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}