using Database.Models;

namespace Database.Services
{
	public class BlipService
	{
		public static List<BlipModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Blips.ToList();
		}

		public static void Add(BlipModel blip)
		{
			using var ctx = new Context();
			ctx.Blips.Add(blip);
			ctx.SaveChanges();
		}
	}
}