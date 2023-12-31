using Database.Models;

namespace Database.Services
{
	public static class ReportService
	{
		public static ReportModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Reports.FirstOrDefault(x => x.Id == id);
		}

		public static void Add(ReportModel model)
		{
			using var ctx = new Context();
			ctx.Reports.Add(model);
			ctx.SaveChanges();
		}
	}
}
