using Database.Models.Crimes;

namespace Database.Services
{
    public static class CrimeService
	{
		public static void Add(CrimeModel model)
		{
			using var ctx = new Context();
			ctx.Crimes.Add(model);
			ctx.SaveChanges();
		}

		public static void Add(List<CrimeModel> models)
		{
			using var ctx = new Context();
			ctx.Crimes.AddRange(models);
			ctx.SaveChanges();
		}

		public static void Remove(CrimeModel model)
		{
			using var ctx = new Context();
			ctx.Crimes.Remove(model);
			ctx.SaveChanges();
		}

		public static CrimeModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Crimes.FirstOrDefault(x => x.Id == id);
        }

        public static bool HasPlayerCrimes(int accountId)
        {
            using var ctx = new Context();
            return ctx.Crimes.Any(x => x.AccountId == accountId);
        }

        public static bool HasPlayerJailtimeCrime(int accountId)
        {
            using var ctx = new Context();
            var crimes = ctx.Crimes.Where(x => x.AccountId == accountId).ToList();
			if (crimes.Count == 0) return false;

			var crimeBases = ctx.CrimeBases.ToList();
			foreach (var crime in crimes)
			{
				var crimeBase = crimeBases.FirstOrDefault(x => x.Id == crime.CrimeId);
				if (crimeBase == null || crimeBase.JailTime <= 0) continue;

				return true;
			}

            return false;
        }

        public static List<CrimeModel> GetPlayerCrimes(int accountId)
		{
			using var ctx = new Context();
			return ctx.Crimes.Where(x => x.AccountId == accountId).ToList();
		}

		public static void RemovePlayerCrimes(int accountId)
		{
			using var ctx = new Context();
			ctx.Crimes.RemoveRange(ctx.Crimes.Where(x => x.AccountId == accountId));
			ctx.SaveChanges();
		}

		public static void Update(CrimeModel model)
		{
			using var ctx = new Context();
			ctx.Crimes.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<CrimeModel> models)
		{
			using var ctx = new Context();
			ctx.Crimes.UpdateRange(models);
			ctx.SaveChanges();
		}

		// bases
		public static List<CrimeBaseModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.CrimeBases.ToList();
		}

		public static void AddBase(CrimeBaseModel model)
		{
			using var ctx = new Context();
			ctx.CrimeBases.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveBase(CrimeBaseModel model)
		{
			using var ctx = new Context();
			ctx.CrimeBases.Remove(model);
			ctx.SaveChanges();
		}

		public static CrimeBaseModel? GetBase(int id)
		{
			using var ctx = new Context();
			return ctx.CrimeBases.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateBase(CrimeBaseModel model)
		{
			using var ctx = new Context();
			ctx.CrimeBases.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateBases(IEnumerable<CrimeBaseModel> models)
		{
			using var ctx = new Context();
			ctx.CrimeBases.UpdateRange(models);
			ctx.SaveChanges();
		}

		// groups
		public static List<CrimeGroupModel> GetAllGroups()
		{
			using var ctx = new Context();
			return ctx.CrimeGroups.ToList();
		}

		public static void AddGroup(CrimeGroupModel model)
		{
			using var ctx = new Context();
			ctx.CrimeGroups.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveGroup(CrimeGroupModel model)
		{
			using var ctx = new Context();
			ctx.CrimeGroups.Remove(model);
			ctx.SaveChanges();
		}

		public static CrimeGroupModel? GetGroup(int id)
		{
			using var ctx = new Context();
			return ctx.CrimeGroups.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateGroup(CrimeGroupModel model)
		{
			using var ctx = new Context();
			ctx.CrimeGroups.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateGroups(IEnumerable<CrimeGroupModel> models)
		{
			using var ctx = new Context();
			ctx.CrimeGroups.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}