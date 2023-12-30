using Database.Models.Account;

namespace Database.Services
{
    public static class LoadoutService
	{
		public static void Add(LoadoutModel model)
		{
			using var ctx = new Context();
			ctx.Loadouts.Add(model);
			ctx.SaveChanges();
		}

		public static bool HasWeapon(int accountId, uint weapon)
		{
			using var ctx = new Context();
			return ctx.Loadouts.Any(x => x.AccountId == accountId && x.Hash == weapon);
		}

		public static void AddRange(params LoadoutModel[] models)
		{
			using var ctx = new Context();
			ctx.Loadouts.AddRange(models);
			ctx.SaveChanges();
		}

		public static void AddRange(List<LoadoutModel> models)
		{
			using var ctx = new Context();
			ctx.Loadouts.AddRange(models);
			ctx.SaveChanges();
		}

		public static void AddAttatchment(LoadoutAttatchmentModel model)
		{
			using var ctx = new Context();
			ctx.LoadoutAttatchments.Add(model);
			ctx.SaveChanges();
		}

		public static LoadoutModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Loadouts.FirstOrDefault(x => x.Id == id);
		}

		public static LoadoutModel? GetLoadout(int accountId, uint hash)
		{
			using var ctx = new Context();
			return ctx.Loadouts.FirstOrDefault(x => x.AccountId == accountId && x.Hash == hash);
		}

		public static List<LoadoutAttatchmentModel> GetLoadoutAttatchments(int loadoutId)
		{
			using var ctx = new Context();
			return ctx.LoadoutAttatchments.Where(x => x.LoadoutId == loadoutId).ToList();
		}

		public static List<LoadoutModel> GetPlayerLoadout(int accId)
		{
			using var ctx = new Context();
			return ctx.Loadouts.Where(x => x.AccountId == accId).ToList();
		}

		public static void Remove(LoadoutModel model)
		{
			using var ctx = new Context();
			ctx.Loadouts.Remove(model);
			ctx.SaveChanges();
		}

		public static void RemoveAttatchment(LoadoutAttatchmentModel model)
		{
			using var ctx = new Context();
			ctx.LoadoutAttatchments.Remove(model);
			ctx.SaveChanges();
		}
		public static void RemoveAttatchments(IEnumerable<LoadoutAttatchmentModel> models)
		{
			using var ctx = new Context();
			ctx.LoadoutAttatchments.RemoveRange(models);
			ctx.SaveChanges();
		}

		public static void ClearPlayerLoadout(int accId, bool federalOnly = false)
		{
			using var ctx = new Context();
			var weapons = ctx.Loadouts.Where(x => x.AccountId == accId && (!federalOnly || x.Type == Core.Enums.LoadoutType.FEDERAL));
			var weaponsIds = weapons.Select(x => x.Id);
			var attatchments = ctx.LoadoutAttatchments.Where(x => weaponsIds.Contains(x.LoadoutId));
			ctx.Loadouts.RemoveRange(weapons);
			ctx.LoadoutAttatchments.RemoveRange(attatchments);
			ctx.SaveChanges();
		}

		public static void Update(LoadoutModel model)
		{
			using var ctx = new Context();
			ctx.Loadouts.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<LoadoutModel> models)
		{
			using var ctx = new Context();
			ctx.Loadouts.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}