using Database.Models.Gangwar;

namespace Database.Services
{
    public static class GangwarService
	{
		public static List<GangwarModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Gangwars.ToList();
		}

		public static List<GangwarModel> GetFromTeam(int teamId)
		{
			using var ctx = new Context();
			return ctx.Gangwars.Where(x => x.OwnerId == teamId).ToList();
		}

		public static List<GangwarSpawnModel> GetSpawns(int gangwarId, bool team)
		{
			using var ctx = new Context();
			return ctx.GangwarSpawns.Where(x => x.GangwarId == gangwarId && x.Team == team).ToList();
		}

		public static void Add(GangwarModel model)
		{
			using var ctx = new Context();
			ctx.Gangwars.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(GangwarModel model)
		{
			using var ctx = new Context();
			ctx.Gangwars.Remove(model);
			ctx.SaveChanges();
		}

		public static GangwarModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Gangwars.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(GangwarModel model)
		{
			using var ctx = new Context();
			ctx.Gangwars.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<GangwarModel> models)
		{
			using var ctx = new Context();
			ctx.Gangwars.UpdateRange(models);
			ctx.SaveChanges();
		}

		public static void AddSpawn(GangwarSpawnModel model)
		{
			using var ctx = new Context();
			ctx.GangwarSpawns.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveSpawn(GangwarSpawnModel model)
		{
			using var ctx = new Context();
			ctx.GangwarSpawns.Remove(model);
			ctx.SaveChanges();
		}

		public static GangwarSpawnModel? GetSpawn(int id)
		{
			using var ctx = new Context();
			return ctx.GangwarSpawns.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateSpawn(GangwarSpawnModel model)
		{
			using var ctx = new Context();
			ctx.GangwarSpawns.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateSpawns(IEnumerable<GangwarSpawnModel> models)
		{
			using var ctx = new Context();
			ctx.GangwarSpawns.UpdateRange(models);
			ctx.SaveChanges();
		}

		public static List<GangwarFlagModel> GetFlags(int gwId)
		{
			using var ctx = new Context();
			return ctx.GangwarFlags.Where(x => x.GangwarId == gwId).ToList();
		}

		public static void AddFlag(GangwarFlagModel model)
		{
			using var ctx = new Context();
			ctx.GangwarFlags.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveFlag(GangwarFlagModel model)
		{
			using var ctx = new Context();
			ctx.GangwarFlags.Remove(model);
			ctx.SaveChanges();
		}

		public static GangwarFlagModel? GetFlag(int id)
		{
			using var ctx = new Context();
			return ctx.GangwarFlags.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateFlag(GangwarFlagModel model)
		{
			using var ctx = new Context();
			ctx.GangwarFlags.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateFlags(IEnumerable<GangwarFlagModel> models)
		{
			using var ctx = new Context();
			ctx.GangwarFlags.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}