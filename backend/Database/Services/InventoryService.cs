using Database.Models.Inventory;

namespace Database.Services
{
    public static class InventoryService
	{
		// Inventory
		public static void Add(InventoryModel model)
		{
			using var ctx = new Context();
			ctx.Inventories.Add(model);
			ctx.SaveChanges();
		}

		public static void Add(params InventoryModel[] models)
		{
			using var ctx = new Context();
			ctx.Inventories.AddRange(models);
			ctx.SaveChanges();
		}

		public static InventoryModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Inventories.FirstOrDefault(x => x.Id == id);
		}

		public static List<InventoryModel> Get(params int[] ids)
		{
			using var ctx = new Context();
			return ctx.Inventories.Where(x => ids.Contains(x.Id)).ToList();
		}

		public static List<InventoryItemModel> GetItemsFromId(int inventoryId, params int[] ids)
		{
			using var ctx = new Context();
			return ctx.InventoryItems.Where(x => x.InventoryId == inventoryId && ids.Contains(x.ItemId)).ToList();
		}

		public static List<InventoryItemModel> GetItemsFromBase(int inventoryId, int baseItemId)
		{
			using var ctx = new Context();
			return ctx.InventoryItems.Where(x => x.InventoryId == inventoryId && x.ItemId == baseItemId).ToList();
		}

		public static void Remove(InventoryModel model)
		{
			using var ctx = new Context();
			ctx.Inventories.Remove(model);
			ctx.SaveChanges();
		}

		public static void Update(InventoryModel model)
		{
			using var ctx = new Context();
			ctx.Inventories.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<InventoryModel> models)
		{
			using var ctx = new Context();
			ctx.Inventories.UpdateRange(models);
			ctx.SaveChanges();
		}

		// Inventory Items

		public static int HasItems(int inventoryId, int itemId)
		{
			using var ctx = new Context();
			return ctx.InventoryItems.Where(x => x.InventoryId == inventoryId && x.ItemId == itemId).Select(x => x.Amount).Sum();
		}

		public static void AddInventoryItem(InventoryItemModel model)
		{
			using var ctx = new Context();
			ctx.InventoryItems.Add(model);
			ctx.SaveChanges();
		}

		public static void AddInventoryItems(IEnumerable<InventoryItemModel> models)
		{
			using var ctx = new Context();
			ctx.InventoryItems.AddRange(models);
			ctx.SaveChanges();
		}

		public static void AddInventoryItems(params InventoryItemModel[] models)
		{
			using var ctx = new Context();
			ctx.InventoryItems.AddRange(models);
			ctx.SaveChanges();
		}

		public static InventoryItemModel? GetInventoryItem(int inventoryId, int id)
		{
			using var ctx = new Context();
			return ctx.InventoryItems.FirstOrDefault(x => x.InventoryId == inventoryId && x.Id == id);
		}

		public static InventoryItemModel? GetInventoryItemBySlot(int inventoryId, int slot)
		{
			using var ctx = new Context();
			return ctx.InventoryItems.FirstOrDefault(x => x.InventoryId == inventoryId && x.Slot == slot);
		}

		public static List<InventoryItemModel> GetInventoryItems(int inventoryId)
		{
			using var ctx = new Context();
			return ctx.InventoryItems.Where(x => x.InventoryId == inventoryId).ToList();
		}

		public static (List<InventoryItemModel> Items1, List<InventoryItemModel>? Items2) GetInventoryItems2(int inventoryId, int? containerId)
		{
			using var ctx = new Context();
			var items1 = ctx.InventoryItems.Where(x => x.InventoryId == inventoryId).ToList();
			var items2 = containerId == null ? null : ctx.InventoryItems.Where(x => x.InventoryId == containerId).ToList();
			return (items1, items2);
		}

		public static List<InventoryItemModel> GetInventoryItemsOfBase(int inventoryId, int itemId)
		{
			using var ctx = new Context();
			return ctx.InventoryItems.Where(x => x.InventoryId == inventoryId && x.ItemId == itemId).ToList();
		}

		public static List<InventoryItemModel> GetInventoryItemsWithSlot(int inventoryId, params int[] slots)
		{
			using var ctx = new Context();
			return ctx.InventoryItems.Where(x => x.InventoryId == inventoryId && slots.Contains(x.Slot)).ToList();
		}

		public static void RemoveInventoryItem(InventoryItemModel model)
		{
			using var ctx = new Context();
			ctx.InventoryItems.Remove(model);
			ctx.SaveChanges();
		}

		public static void RemoveInventoryItems(IEnumerable<InventoryItemModel> models)
		{
			using var ctx = new Context();
			ctx.InventoryItems.RemoveRange(models);
			ctx.SaveChanges();
		}

		public static void UpdateInventoryItem(InventoryItemModel model)
		{
			using var ctx = new Context();
			ctx.InventoryItems.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateInventoryItems(params InventoryItemModel[] models)
		{
			using var ctx = new Context();
			ctx.InventoryItems.UpdateRange(models);
			ctx.SaveChanges();
		}

		public static void UpdateInventoryItems(IEnumerable<InventoryItemModel> models)
		{
			using var ctx = new Context();
			ctx.InventoryItems.UpdateRange(models);
			ctx.SaveChanges();
		}

		public static void ClearInventoryItems(int inventoryId)
		{
			using var ctx = new Context();
			var items = ctx.InventoryItems.Where(x => x.InventoryId == inventoryId);
			ctx.InventoryItems.RemoveRange(items);
			ctx.SaveChanges();
		}

		// Items

		public static void AddItem(ItemModel model)
		{
			using var ctx = new Context();
			ctx.Items.Add(model);
			ctx.SaveChanges();
		}

		public static ItemModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.Items.FirstOrDefault(x => x.Id == id);
		}

		public static List<ItemModel> GetItems()
		{
			using var ctx = new Context();
			return ctx.Items.ToList();
		}

		// Attribute

		public static List<InventoryItemAttributeModel> GetItemAttributes(int inventoryItemId)
		{
			using var ctx = new Context();
			return ctx.InventoryItemAttributes.Where(x => x.InventoryItemId == inventoryItemId).ToList();
		}

		public static InventoryItemAttributeModel? GetItemAttribute(int attributeId)
		{
			using var ctx = new Context();
			return ctx.InventoryItemAttributes.FirstOrDefault(x => x.Id == attributeId);
		}

		public static InventoryItemAttributeModel? GetItemAttributeByItem(int itemId)
		{
			using var ctx = new Context();
			return ctx.InventoryItemAttributes.FirstOrDefault(x => x.InventoryItemId == itemId);
		}

		public static void AddItemAttribute(InventoryItemAttributeModel item)
		{
			using var ctx = new Context();
			ctx.InventoryItemAttributes.Add(item);
			ctx.SaveChanges();
		}

		public static void RemoveItemAttribute(InventoryItemAttributeModel item)
		{
			using var ctx = new Context();
			ctx.InventoryItemAttributes.Remove(item);
			ctx.SaveChanges();
		}

		public static void RemoveItemAttribute(IEnumerable<InventoryItemAttributeModel> items)
		{
			using var ctx = new Context();
			ctx.InventoryItemAttributes.RemoveRange(items);
			ctx.SaveChanges();
		}
	}
}