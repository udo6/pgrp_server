using Core.Enums;
using Database.Models.Inventory;
using Database.Models.Warehouse;

namespace Database.Services
{
    public static class WarehouseService
	{
		public static List<WarehouseModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Warehouses.ToList();
		}

		public static bool HasWarehouse(int owner, OwnerType type)
		{
			using var ctx = new Context();
			return ctx.Warehouses.Any(x => x.OwnerId == owner && x.OwnerType == type);
		}

		public static void Add(WarehouseModel model)
		{
			using var ctx = new Context();
			ctx.Warehouses.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(WarehouseModel model)
		{
			using var ctx = new Context();
			ctx.Warehouses.Remove(model);
			ctx.SaveChanges();
		}

		public static WarehouseModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Warehouses.FirstOrDefault(x => x.Id == id);
		}

		public static WarehouseModel? GetByInventoryId(int id)
		{
			using var ctx = new Context();
			var warehouseInv = ctx.WarehouseInventories.FirstOrDefault(x => x.InventoryId == id);
			if (warehouseInv == null) return null;
			return ctx.Warehouses.FirstOrDefault(e => e.Id == warehouseInv.WarehouseId);
		}

		public static WarehouseModel? GetByOwner(int accountid, OwnerType type)
		{
			using var ctx = new Context();
			return ctx.Warehouses.FirstOrDefault(x => x.OwnerId == accountid && x.OwnerType == type);
		}

		public static void Update(WarehouseModel model)
		{
			using var ctx = new Context();
			ctx.Warehouses.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<WarehouseModel> models)
		{
			using var ctx = new Context();
			ctx.Warehouses.UpdateRange(models);
			ctx.SaveChanges();
		}

		// inventories

		public static List<InventoryModel> GetWarehouseInventories(int ownerId, OwnerType type)
		{
			using var ctx = new Context();
			var warehouse = ctx.Warehouses.FirstOrDefault(x => x.OwnerId == ownerId && x.OwnerType == type);
			if (warehouse == null) return new();

			var warehouseInventories = ctx.WarehouseInventories.Where(x => x.WarehouseId == warehouse.Id).ToList();
			var inventories = new List<InventoryModel>();
			foreach (var inv in warehouseInventories)
			{
				var inventory = ctx.Inventories.FirstOrDefault(x => x.Id == inv.InventoryId);
				if (inventory == null) continue;

				inventories.Add(inventory);
			}

			return inventories;
		}

		public static List<WarehouseInventoryModel> GetFromWarehouse(int warehouseId)
		{
			using var ctx = new Context();
			return ctx.WarehouseInventories.Where(x => x.WarehouseId == warehouseId).ToList();
		}

		public static void AddInventory(WarehouseInventoryModel model)
		{
			using var ctx = new Context();
			ctx.WarehouseInventories.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveInventory(WarehouseInventoryModel model)
		{
			using var ctx = new Context();
			ctx.WarehouseInventories.Remove(model);
			ctx.SaveChanges();
		}

		public static WarehouseInventoryModel? GetInventory(int id)
		{
			using var ctx = new Context();
			return ctx.WarehouseInventories.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateInventory(WarehouseInventoryModel model)
		{
			using var ctx = new Context();
			ctx.WarehouseInventories.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateInventories(IEnumerable<WarehouseInventoryModel> models)
		{
			using var ctx = new Context();
			ctx.WarehouseInventories.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}