using Database.Models.Bank;
using Database.Models.Hospital;

namespace Database.Services
{
	public static class HospitalService
	{
		public static List<HospitalModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Hospitals.ToList();
		}

		public static void Add(HospitalModel model)
		{
			using var ctx = new Context();
			ctx.Hospitals.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(HospitalModel model)
		{
			using var ctx = new Context();
			ctx.Hospitals.Remove(model);
			ctx.SaveChanges();
		}

		public static HospitalModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Hospitals.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(HospitalModel model)
		{
			using var ctx = new Context();
			ctx.Hospitals.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<HospitalModel> models)
		{
			using var ctx = new Context();
			ctx.Hospitals.UpdateRange(models);
			ctx.SaveChanges();
		}

		// beds
		public static List<HospitalBedModel> GetBeds(int hospitalId)
		{
			using var ctx = new Context();
			return ctx.HospitalBeds.Where(x => x.HospitalId == hospitalId).ToList();
		}

		public static void AddBed(HospitalBedModel model)
		{
			using var ctx = new Context();
			ctx.HospitalBeds.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveBed(HospitalBedModel model)
		{
			using var ctx = new Context();
			ctx.HospitalBeds.Remove(model);
			ctx.SaveChanges();
		}

		public static HospitalBedModel? GetBed(int id)
		{
			using var ctx = new Context();
			return ctx.HospitalBeds.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateBed(HospitalBedModel model)
		{
			using var ctx = new Context();
			ctx.HospitalBeds.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateBeds(IEnumerable<HospitalBedModel> models)
		{
			using var ctx = new Context();
			ctx.HospitalBeds.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}