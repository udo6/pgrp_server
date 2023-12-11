using Database.Models.Processor;

namespace Database.Services
{
    public static class ProcessorService
	{
		public static List<ProcessorModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Processors.ToList();
		}

		public static void Add(ProcessorModel model)
		{
			using var ctx = new Context();
			ctx.Processors.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(ProcessorModel model)
		{
			using var ctx = new Context();
			ctx.Processors.Remove(model);
			ctx.SaveChanges();
		}

		public static ProcessorModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Processors.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(ProcessorModel model)
		{
			using var ctx = new Context();
			ctx.Processors.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<ProcessorModel> models)
		{
			using var ctx = new Context();
			ctx.Processors.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}