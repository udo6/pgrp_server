using Database.Models.Bank;
using Database.Models.FFA;

namespace Database.Services
{
	public static class FFAService
	{
		public static List<FFAModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.FFAs.ToList();
		}

		public static void Add(FFAModel model)
		{
			using var ctx = new Context();
			ctx.FFAs.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(FFAModel model)
		{
			using var ctx = new Context();
			ctx.FFAs.Remove(model);
			ctx.SaveChanges();
		}

		public static FFAModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.FFAs.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(FFAModel model)
		{
			using var ctx = new Context();
			ctx.FFAs.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<FFAModel> models)
		{
			using var ctx = new Context();
			ctx.FFAs.UpdateRange(models);
			ctx.SaveChanges();
		}

		// spawns
		public static List<FFASpawnModel> GetSpawns(int ffaId)
		{
			using var ctx = new Context();
			return ctx.FFASpawns.Where(x => x.FFAId == ffaId).ToList();
		}

		public static void AddSpawn(FFASpawnModel model)
		{
			using var ctx = new Context();
			ctx.FFASpawns.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveSpawn(FFASpawnModel model)
		{
			using var ctx = new Context();
			ctx.FFASpawns.Remove(model);
			ctx.SaveChanges();
		}

		public static FFASpawnModel? GetSpawn(int id)
		{
			using var ctx = new Context();
			return ctx.FFASpawns.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateSpawn(FFASpawnModel model)
		{
			using var ctx = new Context();
			ctx.FFASpawns.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateSpawns(IEnumerable<FFASpawnModel> models)
		{
			using var ctx = new Context();
			ctx.FFASpawns.UpdateRange(models);
			ctx.SaveChanges();
		}

		// weapons
		public static List<FFAWeaponModel> GetWeapons(int ffaId)
		{
			using var ctx = new Context();
			return ctx.FFAWeapons.Where(x => x.FFAId == ffaId).ToList();
		}

		public static void AddWeapon(FFAWeaponModel model)
		{
			using var ctx = new Context();
			ctx.FFAWeapons.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveWeapon(FFAWeaponModel model)
		{
			using var ctx = new Context();
			ctx.FFAWeapons.Remove(model);
			ctx.SaveChanges();
		}
	}
}