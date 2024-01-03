using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;
using Core.Enums;
using Database.Models.Account;
using Database.Models.Inventory;
using Logs;
using System.Diagnostics;
using Core.Extensions;

namespace Game.Modules
{
    public static class InventoryModule
	{
		public static Dictionary<InventoryType, string> ContainerLabels = new()
		{
			{ InventoryType.PLAYER, "Spieler" },
			{ InventoryType.TRUNK, "Kofferraum" },
			{ InventoryType.GLOVEBOX, "Handschuhfach" },
			{ InventoryType.LOCKER, "Schließfach" },

			{ InventoryType.HOUSE, "Haus" },
			{ InventoryType.WAREHOUSE, "Lagerkiste" },

			{ InventoryType.FEDERAL_BANK_ROB, "Staatsbank Loot" },
			{ InventoryType.JEWELERY_ROB, "Juwelier Loot" },
			{ InventoryType.LOOTDROP, "MAZ Kiste" },

			{ InventoryType.LAB_FUEL, "Labor Energie" },
			{ InventoryType.LAB_ROB, "Labor Loot" },
			{ InventoryType.LAB_INPUT, "Labor Eingabe" },
			{ InventoryType.LAB_OUTPUT, "Labor Ausgabe" }
		};

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, int>("Server:Inventory:Open", Open);
			Alt.OnClient<RPPlayer, int, int, int, int>("Server:Inventory:MoveAll", MoveAll);
			Alt.OnClient<RPPlayer, int, int, int, int, int>("Server:Inventory:MoveAmount", MoveAmount);
			Alt.OnClient<RPPlayer, int, int>("Server:Inventory:Use", UseItem);
			Alt.OnClient<RPPlayer, int, int, int>("Server:Inventory:Give", GiveItem);
			Alt.OnClient<RPPlayer, int, int>("Server:Inventory:Throw", ThrowItem);
			Alt.OnClient<RPPlayer, int>("Server:Inventory:QuickUse", QuickUse);
			Alt.OnClient<RPPlayer, int>("Server:Inventory:PackItem", PackItem);
			Alt.OnClient<RPPlayer, int, int>("Server:Inventory:PackTargetItem", PackTargetItem);
			Alt.OnClient<RPPlayer, int, int>("Server:Inventory:DropWeapon", DropWeapon);
		}

		private static void Open(RPPlayer player, int targetId = -1)
		{
			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			InventoryModel? container = null;

			if (!player.IsInVehicle)
			{
				var vehicle = RPVehicle.All.FirstOrDefault(x => !x.Locked && !x.TrunkLocked && x.DbId > 0 && x.Position.Distance(player.Position) <= 4);
				if (vehicle == null)
				{
					var shapes = RPShape.All.Where(x => x.Dimension == player.Dimension && x.Position.Distance(player.Position.Down()) <= x.Size).ToList();

					var shape = shapes.FirstOrDefault(x => x.InventoryId > 0 && CheckOwnerType(player, x) && !x.InventoryLocked);
					if (shape != null)
					{
						container = InventoryService.Get(shape.InventoryId);
					}
					else if(shapes.Count > 0)
					{
						foreach(var _shape in shapes)
						{
							switch (_shape.ShapeType)
							{
								case ColshapeType.LABORATORY_INPUT:
									container = InventoryService.Get(player.LaboratoryInputInventoryId);
									break;
								case ColshapeType.LABORATORY_OUTPUT:
									container = InventoryService.Get(player.LaboratoryOutputInventoryId);
									break;
								case ColshapeType.TEAM:
								case ColshapeType.JAIL:
									container = InventoryService.Get(player.LockerInventoryId);
									break;
							}
						}
					}
				}
				else
				{
					container = InventoryService.Get(vehicle.TrunkId);
				}
			}
			else
			{
				container = InventoryService.Get(((RPVehicle)player.Vehicle).GloveBoxId);
			}

			var inventoryItems = InventoryService.GetInventoryItems2(inventory.Id, container?.Id);

			var itemBases = InventoryService.GetItems();
			var invItems = InventoryController.GetInventoryItems(inventoryItems.Items1, itemBases);
			var label = container == null ? "" : GetContainerLabel(container.Type);
			var ctnItems = InventoryController.GetInventoryItems(inventoryItems.Items2, itemBases);

			player.ShowComponent("Inventory", true, JsonConvert.SerializeObject(new
			{
				Inventory = inventory,
				InventoryItems = invItems,
				ContainerLabel = label,
				Container = container,
				ContainerItems = ctnItems,
				Loadout = Array.Empty<LoadoutModel>(),
				GiveItemTarget = targetId,
				SearchTargetId = 0
			}));
		}

		private static void MoveAll(RPPlayer player, int rootInventoryId, int targetInventoryId, int oldSlot, int newSlot)
		{
			var rootInventory = InventoryService.Get(rootInventoryId);
			var targetInventory = InventoryService.Get(targetInventoryId);
			if (rootInventory == null || targetInventory == null || targetInventory.Slots < newSlot) return;

			var containerInventory = player.InventoryId != rootInventoryId ? rootInventory : targetInventory;

			if (rootInventoryId != targetInventoryId)
			{
				if(rootInventoryId != player.InventoryId && targetInventoryId != player.InventoryId) return;
				if (!HasInventoryAccess(player, containerInventory)) return;

				var warehouseInventory = rootInventory.Type == InventoryType.WAREHOUSE ? rootInventory : targetInventory.Type == InventoryType.WAREHOUSE ? targetInventory : null;
				if (warehouseInventory != null)
				{
					var warehouse = WarehouseService.GetByInventoryId(warehouseInventory.Id);
					if (warehouse != null && warehouse.OwnerType == OwnerType.TEAM)
					{
						var account = AccountService.Get(player.DbId);
						if (account == null || !account.TeamStorage || warehouse.OwnerId != account.TeamId)
						{
							player.Notify("Information", "Du bist nicht berechtigt an das Fraktionslager zu gehen!", NotificationType.ERROR);
							return;
						}
					}
				}
			}

			var item = InventoryService.GetInventoryItemBySlot(rootInventoryId, oldSlot);
			if (item == null) return;

			if (rootInventoryId == targetInventoryId)
			{
				InventoryController.MoveAllInsideContainer(rootInventoryId, oldSlot, newSlot);
				return;
			}

			InventoryController.MoveAllToContainer(player, rootInventoryId, targetInventoryId, oldSlot, newSlot);

			LogService.LogInventoryMove(rootInventoryId, targetInventoryId, item.ItemId, item.Amount, Logs.Enums.InventoryMoveType.MOVE);
		}

		private static void MoveAmount(RPPlayer player, int rootInventoryId, int targetInventoryId, int oldSlot, int newSlot, int amount)
		{
			if (amount < 1) return;

			var rootInventory = InventoryService.Get(rootInventoryId);
			var targetInventory = InventoryService.Get(targetInventoryId);
			if (rootInventory == null || targetInventory == null || targetInventory.Slots < newSlot) return;

			var containerInventory = player.InventoryId != rootInventoryId ? rootInventory : targetInventory;

			if (rootInventoryId != targetInventoryId)
			{
				if (rootInventoryId != player.InventoryId && targetInventoryId != player.InventoryId) return;
				if (!HasInventoryAccess(player, containerInventory)) return;

				var warehouseInventory = rootInventory.Type == InventoryType.WAREHOUSE ? rootInventory : targetInventory.Type == InventoryType.WAREHOUSE ? targetInventory : null;
				if (warehouseInventory != null)
				{
					var warehouse = WarehouseService.GetByInventoryId(warehouseInventory.Id);
					if (warehouse != null && warehouse.OwnerType == OwnerType.TEAM)
					{
						var account = AccountService.Get(player.DbId);
						if (account == null || !account.TeamStorage || warehouse.OwnerId != account.TeamId)
						{
							player.Notify("Information", "Du bist nicht berechtigt an das Fraktionslager zu gehen!", NotificationType.ERROR);
							return;
						}
					}
				}
			}

			var item = InventoryService.GetInventoryItemBySlot(rootInventoryId, oldSlot);

			if (rootInventoryId == targetInventoryId)
			{
				InventoryController.MoveAmountInsideContainer(player, rootInventoryId, oldSlot, newSlot, amount);
				return;
			}

			InventoryController.MoveAmountToContainer(player, rootInventoryId, targetInventoryId, oldSlot, newSlot, amount);

			if (item == null) return;
			LogService.LogInventoryMove(rootInventoryId, targetInventoryId, item.ItemId, amount, Logs.Enums.InventoryMoveType.MOVE);
		}

		private static void UseItem(RPPlayer player, int slot, int amount)
		{
			if (amount < 1) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = InventoryService.GetInventoryItemBySlot(player.InventoryId, slot);
			if (item == null || item.Amount < amount) return;

			var script = InventoryController.GetItemScript(item.ItemId);
			if (script == null)
			{
				player.ShowComponent("Inventory", false);
				return;
			}

			if(script.CloseUI)
				player.ShowComponent("Inventory", false);

			script.OnUse(player, inventory, item, slot, amount);
		}

		private static void QuickUse(RPPlayer player, int slot)
		{
			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null || slot > inventory.Slots) return;

			var item = InventoryService.GetInventoryItemBySlot(player.InventoryId, slot);
			if (item == null) return;

			var script = InventoryController.GetItemScript(item.ItemId);
			if (script == null) return;

			script.OnUse(player, inventory, item, slot, 1);
		}

		private static void PackItem(RPPlayer player, int item)
		{
			if (player.Interaction) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			switch (item)
			{
				case 1:
					if (!account.Phone) return;
					StorePhone(player, account);
					break;
				case 2:
					if (!account.Laptop) return;
					StoreLaptop(player, account);
					break;
				case 3:
					if (!account.Backpack) return;
					StoreBackpack(player, account);
					break;
				case 4:
					StoreWeapon(player);
					break;
				case 5:
					StoreVest(player);
					break;
			}
		}

		private static void PackTargetItem(RPPlayer player, int targetId, int item)
		{
			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null || target.Position.Distance(player.Position) > 5f) return;

			var targetAccount = AccountService.Get(target.DbId);
			if (targetAccount == null || (targetAccount.Alive && !targetAccount.Cuffed && !targetAccount.Roped)) return;

			switch (item)
			{
				case 1:
					if (!targetAccount.Phone || !InventoryController.AddItem(target.InventoryId, 12, 1)) return;

					player.ShowComponent("Inventory", false);
					target.Phone = false;
					targetAccount.Phone = false;
					AccountService.Update(targetAccount);
					break;
				case 2:
					if (!targetAccount.Laptop || !InventoryController.AddItem(target.InventoryId, 13, 1)) return;

					player.ShowComponent("Inventory", false);
					target.Laptop = false;
					targetAccount.Laptop = false;
					AccountService.Update(targetAccount);
					break;
			}
		}

		private static void DropWeapon(RPPlayer player, int targetId, int loadoutId)
		{
			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.DbId == targetId);
			if (target == null || player.Position.Distance(target.Position) > 5f) return;

			var targetAccount = AccountService.Get(target.DbId);
			if (targetAccount == null || (targetAccount.Alive && !targetAccount.Cuffed && !targetAccount.Roped)) return;

			var loadout = LoadoutService.Get(loadoutId);
			if (loadout == null) return;

			target.DeleteWeapon(loadout.Hash);
			LoadoutService.Remove(loadout);
			target.Notify("Information", "Dir wurde eine Waffe abgenommen!", NotificationType.ERROR);
			player.Notify("Information", "Du hast eine Waffe weg geworfen!", NotificationType.ERROR);
		}

		private static void StorePhone(RPPlayer player, AccountModel account)
		{
			if (!InventoryController.AddItem(player.InventoryId, 12, 1)) return;

			player.ShowComponent("Inventory", false);
			player.Phone = false;
			account.Phone = false;
			AccountService.Update(account);
		}

		private static void StoreLaptop(RPPlayer player, AccountModel account)
		{
			if (!InventoryController.AddItem(player.InventoryId, 13, 1)) return;

			player.ShowComponent("Inventory", false);
			player.Laptop = false;
			account.Laptop = false;
			AccountService.Update(account);
		}

		private static void StoreBackpack(RPPlayer player, AccountModel account)
		{
			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var items = InventoryService.GetInventoryItems(player.InventoryId);
			if(items.Count > 7)
			{
				player.Notify("Inventar", "Es sind zu viele Slots belegt! (Max. 7)", NotificationType.ERROR);
				return;
			}

			for(var i = 0; i < items.Count; i++)
				items[i].Slot = i + 1;

			InventoryService.UpdateInventoryItems(items);

			if (!InventoryController.AddItem(player.InventoryId, 14, 1)) return;

			player.ShowComponent("Inventory", false);

			account.Backpack = false;
			AccountService.Update(account);

			inventory.Slots = 6;
			inventory.MaxWeight = 25f;
			InventoryService.Update(inventory);
		}

		private static void StoreWeapon(RPPlayer player)
		{
			if (player.CurrentWeapon == 2725352035 || player.IsInVehicle) return;

			var loadout = LoadoutService.GetLoadout(player.DbId, player.CurrentWeapon);
			if (loadout == null) return;

			var attatchments = LoadoutService.GetLoadoutAttatchments(loadout.Id);

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var items = InventoryService.GetItems();
			var inventoryItems = InventoryService.GetInventoryItems(player.InventoryId);

			ItemModel? weaponItem = null;
			ItemModel? ammoItem = null;
			int magSize = 30;
			List<ItemModel> attatchmentItems = new();

			foreach (var item in items)
			{
				var script = InventoryController.GetItemScript(item.Id);
				if (script == null) continue;

				var federal = loadout.Type == LoadoutType.FEDERAL;

				if (script.Type == ItemType.WEAPON)
				{
					var weapon = (WeaponItemScript)script;
					if (weapon.Hash == player.CurrentWeapon && weapon.Federal == federal)
					{
						weaponItem = item;
					}
				}

				if (script.Type == ItemType.AMMO)
				{
					var ammo = (AmmoItemScript)script;
					if (ammo.WeaponHash == player.CurrentWeapon && ammo.Federal == federal)
					{
						ammoItem = item;
						magSize = ammo.MagSize;
					}
				}

				if(script.Type == ItemType.ATTATCHMENT)
				{
					var at = (AttatchmentItemScript)script;
					if(at.Federal == player.TeamDuty && attatchments.FirstOrDefault(e => e.Hash == at.Hash) != null)
					{
						attatchmentItems.Add(item);
					}
				}

				if (weaponItem != null && ammoItem != null && attatchmentItems.Count == attatchments.Count) break;
			}

			if(weaponItem == null)
			{
				player.Notify("Inventar", "Es ist ein Fehler aufgetreten!", NotificationType.ERROR);
				return;
			}

			var weight = InventoryController.CalcInventoryWeight(inventoryItems, items);
			var attatchmentWeight = attatchmentItems.Sum(x => x.Weight);
			var ammoWeight = 0f;
			var ammoSlots = 0;
			var ammoCount = 0;

			if (ammoItem != null)
			{
				ammoCount = (int)Math.Floor((decimal)loadout.Ammo / magSize);
				ammoSlots = (int)Math.Floor((decimal)ammoCount / ammoItem.StackSize);
				ammoWeight = ammoCount * ammoItem.Weight;
			}


			if(inventoryItems.Count + ammoSlots + 1 + attatchmentItems.Count > inventory.Slots || weight + ammoWeight + weaponItem.Weight + attatchmentWeight > inventory.MaxWeight)
			{
				player.Notify("Inventar", "Du hast nicht genug Platz!", NotificationType.ERROR);
				return;
			}

			player.ShowComponent("Inventory", false);

			player.DeleteWeapon(loadout.Hash);
			player.SetAmmo(loadout.Hash, 0);
			InventoryController.AddItem(inventory, weaponItem, 1);
			if(ammoItem != null) InventoryController.AddItem(inventory, ammoItem, ammoCount);
			foreach (var item in attatchmentItems)
				InventoryController.AddItem(inventory, item, 1);

			LoadoutService.Remove(loadout);
			LoadoutService.RemoveAttatchments(attatchments);
		}

		private static void StoreVest(RPPlayer player)
		{
			if (player.Armor < 3 || player.IsInVehicle) return;

			player.ShowComponent("Inventory", false);

			player.PlayAnimation(AnimationType.PACK_VEST);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists || player.Armor < 1) return;

				var clothes = ClothesService.Get(player.ClothesId);
				if (clothes == null) return;

				if(player.VestItemId > 0)
				{
					var inventory = InventoryService.Get(player.InventoryId);
					if (inventory == null) return;

					var slot = InventoryController.GetFirstFreeSlot(inventory);
					if (slot == -1) return;

					var item = new InventoryItemModel(inventory.Id, player.VestItemId, 1, slot, player.Armor < 100);
					InventoryService.AddInventoryItem(item);

					if (player.Armor < 100)
						InventoryService.AddItemAttribute(new(item.Id, player.Armor));
				}

				player.SetArmor(0);
				player.SetClothing(9, 0, 0, 0);
				player.VestItemId = 0;
				clothes.Armor = 0;
				clothes.ArmorColor = 0;
				clothes.ArmorDlc = 0;
				ClothesService.Update(clothes);
			}, 4000);
		}

		private static void GiveItem(RPPlayer player, int targetId, int slot, int amount)
		{
			if (amount < 1) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Id == targetId);
			if (target == null) return;

			var item = InventoryService.GetInventoryItemBySlot(player.InventoryId, slot);
			if (item == null || item.Amount < amount) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			if (!InventoryController.AddItem(target.InventoryId, item.ItemId, amount)) return;
			InventoryController.RemoveItem(inventory, slot, amount);
			player.PlayAnimation(AnimationType.GIVE_ITEM);
			player.Notify("Information", "Du hast jemandem einen Gegenstand zugesteckt!", NotificationType.INFO);
			target.Notify("Information", "Jemand hat dir einen Gegenstand zugesteckt!", NotificationType.INFO);

			LogService.LogInventoryMove(player.InventoryId, target.InventoryId, item.ItemId, amount, Logs.Enums.InventoryMoveType.GIVE);
		}

		private static void ThrowItem(RPPlayer player, int slot, int amount)
		{
			if (amount < 1) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = InventoryService.GetInventoryItemBySlot(player.InventoryId, slot);
			if (item == null) return;

			player.PlayAnimation(AnimationType.GIVE_ITEM);
			InventoryController.RemoveItem(inventory, slot, amount);

			LogService.LogInventoryMove(player.InventoryId, 0, item.ItemId, amount, Logs.Enums.InventoryMoveType.DELETED);
		}

		private static bool CheckOwnerType(RPPlayer player, RPShape shape)
		{
			return shape.InventoryAccess.Count == 0 || shape.InventoryAccess.Any(x => (x.Type == OwnerType.PLAYER && x.Id == player.DbId) || (x.Type == OwnerType.TEAM && x.Id == player.TeamId));
		}

		public static string GetContainerLabel(InventoryType type)
		{
			if (!ContainerLabels.ContainsKey(type)) return "Unbekannt";

			return ContainerLabels[type];
		}

		public static bool HasInventoryAccess(RPPlayer player, InventoryModel inventory)
		{
			switch(inventory.Type)
			{
				case InventoryType.PLAYER:
					var targetPlayer = RPPlayer.All.FirstOrDefault(x => x.InventoryId == inventory.Id && x.Dimension == player.Dimension);
					return targetPlayer != null && (!targetPlayer.Alive || targetPlayer.Roped || targetPlayer.Cuffed) && player.Position.Distance(targetPlayer.Position) < 5f;
				case InventoryType.TRUNK:
					var targetVehicle = RPVehicle.All.FirstOrDefault(x => x.TrunkId == inventory.Id && x.Dimension == player.Dimension);
					return targetVehicle != null && !targetVehicle.Locked && !targetVehicle.TrunkLocked && player.Position.Distance(targetVehicle.Position) < 5f;
				case InventoryType.GLOVEBOX:
					return player.IsInVehicle && ((RPVehicle)player.Vehicle).GloveBoxId == inventory.Id && player.Position.Distance(player.Vehicle.Position) < 5f;
				case InventoryType.LAB_FUEL:
					var labFuelShape = RPShape.All.FirstOrDefault(x => x.ShapeType == ColshapeType.LABORATORY_FUEL && x.InventoryId == inventory.Id && x.Dimension == player.Dimension && x.InventoryAccess.Count > 0 && x.InventoryAccess[0].Id == player.TeamId);
					return labFuelShape != null && player.Position.Distance(labFuelShape.Position) < labFuelShape.Size;
				case InventoryType.LAB_INPUT:
					return player.LaboratoryInputInventoryId == inventory.Id;
				case InventoryType.LAB_OUTPUT:
					return player.LaboratoryOutputInventoryId == inventory.Id;
				case InventoryType.LAB_ROB:
					var labRobShape = RPShape.All.FirstOrDefault(x => x.ShapeType == ColshapeType.LABORATORY_ROB && x.InventoryId == inventory.Id && x.Dimension == player.Dimension);
					return labRobShape != null && player.Position.Distance(labRobShape.Position) < labRobShape.Size;
				case InventoryType.LOCKER:
					return player.LockerInventoryId == inventory.Id;
				case InventoryType.HOUSE:
					var houseShape = RPShape.All.FirstOrDefault(x => x.ShapeType == ColshapeType.HOUSE_INVENTORY && x.InventoryId == inventory.Id && x.Dimension == player.Dimension);
					return houseShape != null && houseShape.InventoryAccess.Any(x => (x.Id == player.DbId && x.Type == OwnerType.PLAYER) || (x.Id == player.TeamId && x.Type == OwnerType.TEAM)) && player.Position.Distance(houseShape.Position) < houseShape.Size;
				case InventoryType.WAREHOUSE:
					var warehouseShape = RPShape.All.FirstOrDefault(x => x.ShapeType == ColshapeType.WAREHOUSE_BOX && x.InventoryId == inventory.Id && x.Dimension == player.Dimension);
					return warehouseShape != null && warehouseShape.InventoryAccess.Any(x => (x.Id == player.DbId && x.Type == OwnerType.PLAYER) || (x.Id == player.TeamId && x.Type == OwnerType.TEAM)) && player.Position.Distance(warehouseShape.Position) < warehouseShape.Size;
				case InventoryType.FEDERAL_BANK_ROB:
					var bankRobberyShape = RPShape.All.FirstOrDefault(x => x.ShapeType == ColshapeType.FEDERAL_BANK_ROBBERY_LOOT && x.InventoryId == inventory.Id && x.Dimension == player.Dimension);
					return bankRobberyShape != null && player.Position.Distance(bankRobberyShape.Position) < bankRobberyShape.Size;
				case InventoryType.JEWELERY_ROB:
					var jewleryRobberyShape = RPShape.All.FirstOrDefault(x => x.ShapeType == ColshapeType.JEWELERY_LOOT && x.InventoryId == inventory.Id && x.Dimension == player.Dimension);
					return jewleryRobberyShape != null && player.Position.Distance(jewleryRobberyShape.Position) < jewleryRobberyShape.Size;
			}

			return false;
		}
	}
}