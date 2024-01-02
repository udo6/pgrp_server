using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;

namespace Game.Controllers
{
    public static class InventoryController
	{
		public static List<ItemScript> ItemScripts = new();

		public static List<WeaponItemScript> GetWeaponItemScripts()
		{
			return ItemScripts.Where(x => x.Type == ItemType.WEAPON).Cast<WeaponItemScript>().ToList();
		}

		public static ItemScript? GetItemScript(InventoryItemModel item)
		{
			return ItemScripts.FirstOrDefault(x => x.ItemId == item.ItemId);
		}

		public static ItemScript? GetItemScript(int itemId)
		{
			return ItemScripts.FirstOrDefault(x => x.ItemId == itemId);
		}

		public static void MoveAllInsideContainer(int inventoryId, int oldSlot, int newSlot)
		{
			var inventory = InventoryService.Get(inventoryId);
			if (inventory == null) return;

			var items = InventoryService.GetInventoryItemsWithSlot(inventoryId, oldSlot, newSlot);
			if (items.Count < 1) return;

			if (items.Count == 1)
			{
				if(items[0].Slot == oldSlot)
				{
					items[0].Slot = newSlot;
					InventoryService.UpdateInventoryItem(items[0]);
				}
				return;
			}

			var rootItem = items[0].Slot == oldSlot ? items[0] : items[1];
			var targetItem = items[0].Slot == newSlot ? items[0] : items[1];

			var targetItemBase = InventoryService.GetItem(targetItem.ItemId);
			if (targetItemBase == null) return;

			if (rootItem.ItemId != targetItem.ItemId)
			{
				rootItem.Slot = newSlot;
				targetItem.Slot = oldSlot;
				InventoryService.UpdateInventoryItems(rootItem, targetItem);
				return;
			}

			if (rootItem.HasAttribute || targetItem.HasAttribute) return;

			var space = targetItemBase.StackSize - targetItem.Amount;
			if (space <= 0) return;

			if (space <= rootItem.Amount)
			{
				targetItem.Amount += space;
				rootItem.Amount -= space;

				if (rootItem.Amount <= 0)
				{
					InventoryService.RemoveInventoryItem(rootItem);
					InventoryService.UpdateInventoryItem(targetItem);
					return;
				}

				InventoryService.UpdateInventoryItems(rootItem, targetItem);
				return;
			}

			targetItem.Amount += rootItem.Amount;
			InventoryService.RemoveInventoryItem(rootItem);
			InventoryService.UpdateInventoryItem(targetItem);
		}

		public static void MoveAllToContainer(RPPlayer player, int rootInventoryId, int targetInventoryId, int oldSlot, int newSlot)
		{
			var inventories = InventoryService.Get(rootInventoryId, targetInventoryId);
			if (inventories.Count != 2) return;

			var rootInventory = inventories[0].Id == rootInventoryId ? inventories[0] : inventories[1];
			var targetInventory = inventories[0].Id == targetInventoryId ? inventories[0] : inventories[1];

			var rootItem = InventoryService.GetInventoryItemBySlot(rootInventoryId, oldSlot);
			if (rootItem == null) return;

			var rootItemBase = InventoryService.GetItem(rootItem.ItemId);
			if (rootItemBase == null) return;

			var targetItem = InventoryService.GetInventoryItemBySlot(targetInventoryId, newSlot);

			// add checks like team storage perms here

			var rootInventoryWeight = CalcInventoryWeight(rootInventory);
			var targetInventoryWeight = CalcInventoryWeight(targetInventory);

			if (targetItem != null)
			{
				var targetItemBase = InventoryService.GetItem(targetItem.ItemId);
				if (targetItemBase == null) return;

				if (rootItem.ItemId == targetItem.ItemId && (!rootItem.HasAttribute && !targetItem.HasAttribute))
				{
					var diff = targetItemBase.StackSize - targetItem.Amount;
					if (targetInventoryWeight + diff * targetItemBase.Weight > targetInventory.MaxWeight) return;

					if (rootItem.Amount <= diff)
					{
						targetItem.Amount += rootItem.Amount;
						InventoryService.RemoveInventoryItem(rootItem);
						InventoryService.UpdateInventoryItem(targetItem);
						return;
					}

					rootItem.Amount -= diff;
					targetItem.Amount += diff;
					player.PlayAnimation(AnimationType.GIVE_ITEM);
					InventoryService.UpdateInventoryItems(rootItem, targetItem);
					return;
				}

				if (rootInventoryWeight - (rootItemBase.Weight * rootItem.Amount) + (targetItemBase.Weight * targetItem.Amount) > rootInventory.MaxWeight)
				{
					player.ShowComponent("Inventory", false);
					player.Notify("System", "Es ist ein Fehler aufgetreten!", NotificationType.ERROR);
					return;
				}

				if (targetInventoryWeight - (targetItemBase.Weight * targetItem.Amount) + (rootItemBase.Weight * rootItem.Amount) > targetInventory.MaxWeight)
				{
					player.ShowComponent("Inventory", false);
					player.Notify("System", "Es ist ein Fehler aufgetreten!", NotificationType.ERROR);
					return;
				}

				var rootId = rootItem.Id;
				var rootInv = rootItem.InventoryId;
				var rootItemId = rootItem.ItemId;
				var rootAmount = rootItem.Amount;

				var targetId = targetItem.Id;
				var targetInv = targetItem.InventoryId;
				var targetItemId = targetItem.ItemId;
				var targetAmount = targetItem.Amount;

				rootItem.Id = targetId;
				rootItem.InventoryId = targetInv;
				rootItem.ItemId = targetItemId;
				rootItem.Amount = targetAmount;

				targetItem.Id = rootId;
				targetItem.InventoryId = rootInv;
				targetItem.ItemId = rootItemId;
				targetItem.Amount = rootAmount;
				player.PlayAnimation(AnimationType.GIVE_ITEM);
				InventoryService.UpdateInventoryItems(rootItem, targetItem);
				return;
			}

			if (targetInventoryWeight + rootItemBase.Weight * rootItem.Amount > targetInventory.MaxWeight)
			{
				player.ShowComponent("Inventory", false);
				player.Notify("System", "Es ist ein Fehler aufgetreten!", NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(AnimationType.GIVE_ITEM);
			rootItem.InventoryId = targetInventoryId;
			rootItem.Slot = newSlot;
			InventoryService.UpdateInventoryItem(rootItem);
		}

		public static void MoveAmountInsideContainer(RPPlayer player, int inventoryId, int oldSlot, int newSlot, int amount)
		{
			var inventory = InventoryService.Get(inventoryId);
			if (inventory == null) return;

			var items = InventoryService.GetInventoryItemsWithSlot(inventoryId, oldSlot, newSlot);
			if (items.Count != 1 || items[0].Slot != oldSlot) return;

			var rootItem = items[0];
			if (rootItem.Amount <= amount) return;

			rootItem.Amount -= amount;

			var targetItem = new InventoryItemModel(
				inventoryId,
				rootItem.ItemId,
				amount,
				newSlot,
				false);

			InventoryService.UpdateInventoryItem(rootItem);
			InventoryService.AddInventoryItem(targetItem);
		}

		public static void MoveAmountToContainer(RPPlayer player, int rootInventoryId, int targetInventoryId, int oldSlot, int newSlot, int amount)
		{
			var inventories = InventoryService.Get(rootInventoryId, targetInventoryId);
			if (inventories.Count != 2) return;

			var targetInventory = inventories[0].Id == targetInventoryId ? inventories[0] : inventories[1];

			var rootItem = InventoryService.GetInventoryItemBySlot(rootInventoryId, oldSlot);
			if (rootItem == null || rootItem.Amount <= amount) return;

			var rootItemBase = InventoryService.GetItem(rootItem.ItemId);
			if (rootItemBase == null) return;

			var targetItem = InventoryService.GetInventoryItemBySlot(targetInventoryId, newSlot);
			if (targetItem != null) return;

			// add checks like team storage perms here

			var targetInventoryWeight = CalcInventoryWeight(targetInventory);

			if (targetInventoryWeight + rootItemBase.Weight * rootItem.Amount > targetInventory.MaxWeight)
			{
				player.ShowComponent("Inventory", false);
				player.Notify("System", "Es ist ein Fehler aufgetreten!", NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(AnimationType.GIVE_ITEM);
			rootItem.Amount -= amount;
			InventoryService.UpdateInventoryItem(rootItem);
			InventoryService.AddInventoryItem(new(targetInventoryId, rootItem.ItemId, amount, newSlot, false));
		}

		public static bool AddItem(InventoryModel inventory, ItemModel itemBase, int amount)
		{
			var items = InventoryService.GetInventoryItems(inventory.Id);
			var itemBases = InventoryService.GetItems();
			if (CalcInventoryWeight(items, itemBases) + itemBase.Weight * amount > inventory.MaxWeight) return false;

			var amountLeft = amount;
			var updatedItems = new List<InventoryItemModel>();

			for (var i = 0; i < items.Count; i++)
			{
				if (items[i].ItemId != itemBase.Id) continue;

				if (items[i].Amount + amountLeft > itemBase.StackSize)
				{
					var diff = itemBase.StackSize - items[i].Amount;
					items[i].Amount += diff;
					amountLeft -= diff;
					updatedItems.Add(items[i]);
					continue;
				}

				items[i].Amount += amountLeft;
				amountLeft = 0;
				updatedItems.Add(items[i]);
				break;
			}

			var itemsToAdd = new List<InventoryItemModel>();

			while (amountLeft > 0)
			{
				var slot = GetFirstFreeSlot(inventory, items, itemsToAdd);
				if (slot == -1) return false;

				if (amountLeft > itemBase.StackSize)
				{
					itemsToAdd.Add(new(inventory.Id, itemBase.Id, itemBase.StackSize, slot, false));
					amountLeft -= itemBase.StackSize;
					continue;
				}

				itemsToAdd.Add(new(inventory.Id, itemBase.Id, amountLeft, slot, false));
				break;
			}

			if (updatedItems.Count > 0) InventoryService.UpdateInventoryItems(updatedItems);
			if (itemsToAdd.Count > 0) InventoryService.AddInventoryItems(itemsToAdd);

			return true;
		}

		public static bool AddItem(int inventoryId, int itemBaseId, int amount)
		{
			var inventory = InventoryService.Get(inventoryId);
			if (inventory == null) return false;

			var items = InventoryService.GetInventoryItems(inventory.Id);
			var itemBases = InventoryService.GetItems();
			var itemBase = itemBases.FirstOrDefault(x => x.Id == itemBaseId);
			if (itemBase == null) return false;

			if (CalcInventoryWeight(items, itemBases) + itemBase.Weight * amount > inventory.MaxWeight) return false;

			var amountLeft = amount;
			var updatedItems = new List<InventoryItemModel>();

			for (var i = 0; i < items.Count; i++)
			{
				if (items[i].ItemId != itemBase.Id || items[i].HasAttribute) continue;

				if (items[i].Amount + amountLeft > itemBase.StackSize)
				{
					var diff = itemBase.StackSize - items[i].Amount;
					items[i].Amount += diff;
					amountLeft -= diff;
					updatedItems.Add(items[i]);
					continue;
				}

				items[i].Amount += amountLeft;
				amountLeft = 0;
				updatedItems.Add(items[i]);
				break;
			}

			var itemsToAdd = new List<InventoryItemModel>();

			while (amountLeft > 0)
			{
				var slot = GetFirstFreeSlot(inventory, items, itemsToAdd);
				if (slot == -1) return false;

				if (amountLeft > itemBase.StackSize)
				{
					itemsToAdd.Add(new(inventory.Id, itemBase.Id, itemBase.StackSize, slot, false));
					amountLeft -= itemBase.StackSize;
					continue;
				}

				itemsToAdd.Add(new(inventory.Id, itemBase.Id, amountLeft, slot, false));
				break;
			}

			if (updatedItems.Count > 0) InventoryService.UpdateInventoryItems(updatedItems);
			if (itemsToAdd.Count > 0) InventoryService.AddInventoryItems(itemsToAdd);

			return true;
		}

		public static bool RemoveItem(InventoryModel inventory, int slot, int amount)
		{
			var item = InventoryService.GetInventoryItemBySlot(inventory.Id, slot);
			if (item == null) return false;

			item.Amount -= amount;

			if (item.Amount > 0) InventoryService.UpdateInventoryItem(item);
			else InventoryService.RemoveInventoryItem(item);

			return true;
		}

		public static bool RemoveItem(InventoryModel inventory, ItemModel itemBase, int amount)
		{
			var items = InventoryService.GetInventoryItemsOfBase(inventory.Id, itemBase.Id);

			var itemAmount = items.Where(x => x.ItemId == itemBase.Id).Sum(x => x.Amount);
			if(itemAmount < amount) return false;

			var removedItems = new List<InventoryItemModel>();

			var amountLeft = amount;
			for (var i = 0; i < items.Count; i++)
			{
				if (amountLeft <= 0) break;

				if (amountLeft >= items[i].Amount)
				{
					removedItems.Add(items[i]);
					amountLeft -= items[i].Amount;
					continue;
				}

				items[i].Amount -= amountLeft;
				InventoryService.UpdateInventoryItem(items[i]);
				break;
			}

			InventoryService.RemoveInventoryItems(removedItems);
			return true;
		}

		public static int GetFirstFreeSlot(InventoryModel inventory)
		{
			var items = InventoryService.GetInventoryItems(inventory.Id);
			for (var i = 1; i <= inventory.Slots; i++) if (items.FirstOrDefault(x => x.Slot == i) == null) return i;

			return -1;
		}

		public static int GetFirstFreeSlot(InventoryModel inventory, List<InventoryItemModel> items)
		{
			for (var i = 1; i <= inventory.Slots; i++)
				if (items.FirstOrDefault(x => x.Slot == i) == null) return i;

			return -1;
		}

		private static int GetFirstFreeSlot(InventoryModel inventory, List<InventoryItemModel> items, List<InventoryItemModel> items2)
		{
			for (var i = 1; i <= inventory.Slots; i++)
				if (items.FirstOrDefault(x => x.Slot == i) == null && items2.FirstOrDefault(x => x.Slot == i) == null) return i;

			return -1;
		}

		public static float CalcInventoryWeight(InventoryModel inventory)
		{
			var result = 0f;
			var items = InventoryService.GetInventoryItems(inventory.Id);
			var itemBases = InventoryService.GetItems();

			foreach (var item in items)
			{
				var itemBase = itemBases.FirstOrDefault(x => x.Id == item.ItemId);
				if (itemBase == null) continue;

				result += itemBase.Weight * item.Amount;
			}

			return result;
		}

		public static float CalcInventoryWeight(List<InventoryItemModel> items)
		{
			var result = 0f;
			var itemBases = InventoryService.GetItems();

			foreach (var item in items)
			{
				var itemBase = itemBases.FirstOrDefault(x => x.Id == item.ItemId);
				if (itemBase == null) continue;

				result += itemBase.Weight * item.Amount;
			}

			return result;
		}

		public static float CalcInventoryWeight(List<InventoryItemModel> items, List<ItemModel> itemBases)
		{
			var result = 0f;

			foreach (var item in items)
			{
				var itemBase = itemBases.FirstOrDefault(x => x.Id == item.ItemId);
				if (itemBase == null) continue;

				result += itemBase.Weight * item.Amount;
			}

			return result;
		}

		public static List<object>? GetInventoryItems(List<InventoryItemModel>? items, List<ItemModel> itemBases)
		{
			if (items == null) return null;

			var result = new List<object>();

			foreach (var item in items)
			{
				var itemBase = itemBases.FirstOrDefault(x => x.Id == item.ItemId);
				if (itemBase == null) continue;

				var name = itemBase.Name;

				if (item.HasAttribute)
				{
					var attributes = InventoryService.GetItemAttributes(item.Id);
					foreach (var attribute in attributes)
					{
						name += $" ({attribute.Value})";
					}
				}

				result.Add(new
				{
					Id = item.Id,
					Icon = itemBase.Icon,
					Amount = item.Amount,
					Slot = item.Slot,
					Name = name,
					StackSize = itemBase.StackSize,
					Weight = itemBase.Weight
				});
			}

			return result;
		}
	}
}