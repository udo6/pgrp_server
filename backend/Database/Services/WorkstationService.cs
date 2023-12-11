using Database.Models.Gangwar;
using Database.Models.Workstation;

namespace Database.Services
{
	public static class WorkstationService
	{
		public static List<WorkstationModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Workstations.ToList();
		}

		public static void Add(WorkstationModel model)
		{
			using var ctx = new Context();
			ctx.Workstations.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(WorkstationModel model)
		{
			using var ctx = new Context();
			ctx.Workstations.Remove(model);
			ctx.SaveChanges();
		}

		public static WorkstationModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Workstations.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(WorkstationModel model)
		{
			using var ctx = new Context();
			ctx.Workstations.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<WorkstationModel> models)
		{
			using var ctx = new Context();
			ctx.Workstations.UpdateRange(models);
			ctx.SaveChanges();
		}

		// blueprints
		public static List<WorkstationBlueprintModel> GetBlueprints(int stationId)
		{
			using var ctx = new Context();
			return ctx.WorkstationBlueprints.Where(x => x.WorkstationId == stationId).ToList();
		}

		public static void AddBlueprint(WorkstationBlueprintModel model)
		{
			using var ctx = new Context();
			ctx.WorkstationBlueprints.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveBlueprint(WorkstationBlueprintModel model)
		{
			using var ctx = new Context();
			ctx.WorkstationBlueprints.Remove(model);
			ctx.SaveChanges();
		}

		public static WorkstationBlueprintModel? GetBlueprint(int id)
		{
			using var ctx = new Context();
			return ctx.WorkstationBlueprints.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateBlueprint(WorkstationBlueprintModel model)
		{
			using var ctx = new Context();
			ctx.WorkstationBlueprints.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateBlueprints(IEnumerable<WorkstationBlueprintModel> models)
		{
			using var ctx = new Context();
			ctx.WorkstationBlueprints.UpdateRange(models);
			ctx.SaveChanges();
		}

		// items
		public static List<WorkstationItemModel> GetAllItems()
		{
			using var ctx = new Context();
			return ctx.WorkstationItems.ToList();
		}

		public static List<WorkstationItemModel> GetItems(int accountId, int stationId)
		{
			using var ctx = new Context();
			return ctx.WorkstationItems.Where(x => x.WorkstationId == stationId && x.AccountId == accountId).ToList();
		}

		public static int GetItemsCount(int accountId, int stationId, int itemId)
		{
			using var ctx = new Context();
			return ctx.WorkstationItems.Where(x => x.WorkstationId == stationId && x.AccountId == accountId && x.ItemId == itemId).Count();
		}

		public static void AddItem(WorkstationItemModel model)
		{
			using var ctx = new Context();
			ctx.WorkstationItems.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveItem(WorkstationItemModel model)
		{
			using var ctx = new Context();
			ctx.WorkstationItems.Remove(model);
			ctx.SaveChanges();
		}

		public static WorkstationItemModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.WorkstationItems.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateItem(WorkstationItemModel model)
		{
			using var ctx = new Context();
			ctx.WorkstationItems.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateItems(IEnumerable<WorkstationItemModel> models)
		{
			using var ctx = new Context();
			ctx.WorkstationItems.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}