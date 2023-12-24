using Database.Models.Account;
using Database.Models.Barber;

namespace Database.Services
{
	public static class BarberService
	{
		public static List<BarberModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Barbers.ToList();
		}

		public static void Add(BarberModel model)
		{
			using var ctx = new Context();
			ctx.Barbers.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(BarberModel model)
		{
			using var ctx = new Context();
			ctx.Barbers.Remove(model);
			ctx.SaveChanges();
		}

		public static BarberModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Barbers.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(BarberModel model)
		{
			using var ctx = new Context();
			ctx.Barbers.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<BarberModel> models)
		{
			using var ctx = new Context();
			ctx.Barbers.UpdateRange(models);
			ctx.SaveChanges();
		}

		// styles

		public static List<BarberStyleModel> GetStylesFromBarber(/*int barberId, */int gender)
		{
			using var ctx = new Context();
			return ctx.BarberStyles.Where(x => /*x.BarberId == barberId &&*/ x.Gender == gender).ToList();
		}

		public static List<BarberBeardModel> GetBeardsFromBarber(/*int barberId*/)
		{
			using var ctx = new Context();
			return ctx.BarberBeardModels/*.Where(x => x.BarberId == barberId)*/.ToList();
		}

		public static void AddStyle(BarberStyleModel model)
		{
			using var ctx = new Context();
			ctx.BarberStyles.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveStyle(BarberStyleModel model)
		{
			using var ctx = new Context();
			ctx.BarberStyles.Remove(model);
			ctx.SaveChanges();
		}

		public static BarberStyleModel? GetStyle(int id)
		{
			using var ctx = new Context();
			return ctx.BarberStyles.FirstOrDefault(x => x.Id == id);
		}

		// colors

		public static List<BarberColorModel> GetAllColors()
		{
			using var ctx = new Context();
			return ctx.BarberColors.ToList();
		}

		public static void AddColor(BarberColorModel model)
		{
			using var ctx = new Context();
			ctx.BarberColors.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveColor(BarberColorModel model)
		{
			using var ctx = new Context();
			ctx.BarberColors.Remove(model);
			ctx.SaveChanges();
		}

		public static BarberColorModel? GetColor(int id)
		{
			using var ctx = new Context();
			return ctx.BarberColors.FirstOrDefault(x => x.Id == id);
		}
	}
}