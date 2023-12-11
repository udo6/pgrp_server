using Core.Enums;
using Database.Models.Vehicle;

namespace Database.Services
{
    public static class VehicleService
	{
		public static VehicleModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Vehicles.FirstOrDefault(x => x.Id == id);
		}

		public static List<VehicleModel> GetPlayerVehicles(int accId)
		{
			using var ctx = new Context();
			return ctx.Vehicles.Where(x => x.Type == OwnerType.PLAYER && (x.OwnerId == accId || x.KeyHolderId == accId)).ToList();
		}

		public static List<VehicleModel> GetAllPlayerVehicles(int accId, int teamId, int businessId)
		{
			using var ctx = new Context();
			return ctx.Vehicles.Where(x =>
				(x.Type == OwnerType.PLAYER && (x.OwnerId == accId || x.KeyHolderId == accId)) ||
				(x.Type == OwnerType.TEAM && x.OwnerId == teamId)).ToList();
		}

		public static List<VehicleModel> GetPlayerImpoundedVehicles(int accId)
		{
			using var ctx = new Context();
			return ctx.Vehicles.Where(x => x.Type == OwnerType.PLAYER && (x.OwnerId == accId || x.KeyHolderId == accId) && x.Parked && x.GarageId == 0).ToList();
		}

		public static List<VehicleModel> GetParkedPlayerVehicles(int owner, int garageId, OwnerType type)
		{
			using var ctx = new Context();
			return ctx.Vehicles.Where(x =>
				x.Type == type &&
				(x.OwnerId == owner || (type == OwnerType.PLAYER && x.KeyHolderId == owner)) &&
				x.Parked &&
				x.GarageId == garageId).ToList();
		}

		public static List<VehicleModel> GetParked()
		{
			using var ctx = new Context();
			return ctx.Vehicles.Where(x => !x.Parked).ToList();
		}

		public static List<VehicleBaseModel> GetAllBases()
		{
			using var ctx = new Context();
			return ctx.VehicleBase.ToList();
		}

		public static VehicleBaseModel? GetBase(int id)
		{
			using var ctx = new Context();
			return ctx.VehicleBase.FirstOrDefault(x => x.Id == id);
		}

		public static void AddVehicle(VehicleModel model)
		{
			using var ctx = new Context();
			ctx.Vehicles.Add(model);
			ctx.SaveChanges();
		}

		public static void AddVehicleBase(VehicleBaseModel model)
		{
			using var ctx = new Context();
			ctx.VehicleBase.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveVehicle(VehicleModel model)
		{
			using var ctx = new Context();
			ctx.Vehicles.Remove(model);
			ctx.SaveChanges();
		}

		public static void RemoveVehicleBase(VehicleBaseModel model)
		{
			using var ctx = new Context();
			ctx.VehicleBase.Remove(model);
			ctx.SaveChanges();
		}

		public static void UpdateVehicle(VehicleModel model)
		{
			using var ctx = new Context();
			ctx.Vehicles.Update(model);
			ctx.SaveChanges();
		}
	}
}