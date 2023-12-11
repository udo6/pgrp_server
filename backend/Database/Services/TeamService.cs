using Database.Models.Team;

namespace Database.Services
{
    public static class TeamService
	{
		public static List<TeamModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Teams.ToList();
		}

		public static void Add(TeamModel model)
		{
			using var ctx = new Context();
			ctx.Teams.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(TeamModel model)
		{
			using var ctx = new Context();
			ctx.Teams.Remove(model);
			ctx.SaveChanges();
		}

		public static TeamModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Teams.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(TeamModel model)
		{
			using var ctx = new Context();
			ctx.Teams.Update(model);
			ctx.SaveChanges();
		}

		public static List<LaboratoryModel> GetAllLaboratories()
		{
			using var ctx = new Context();
			return ctx.Laboratories.ToList();
		}

		public static void AddLaboratory(LaboratoryModel model)
		{
			using var ctx = new Context();
			ctx.Laboratories.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveLaboratory(LaboratoryModel model)
		{
			using var ctx = new Context();
			ctx.Laboratories.Remove(model);
			ctx.SaveChanges();
		}

		public static LaboratoryModel? GetLaboratory(int id)
		{
			using var ctx = new Context();
			return ctx.Laboratories.FirstOrDefault(x => x.Id == id);
		}

		public static LaboratoryModel? GetLaboratoryByTeam(int teamId)
		{
			using var ctx = new Context();
			return ctx.Laboratories.FirstOrDefault(x => x.TeamId == teamId);
		}

		public static void UpdateLaboratory(LaboratoryModel model)
		{
			using var ctx = new Context();
			ctx.Laboratories.Update(model);
			ctx.SaveChanges();
		}
	}
}