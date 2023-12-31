using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Inventory;
using Database.Models.Jumpoint;
using Database.Models.Warehouse;
using Database.Services;
using Game.Controllers;
using System.Diagnostics;

namespace Game.Modules
{
	public static class WarehouseModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach (var model in WarehouseService.GetAll())
				WarehouseController.LoadWarehouse(model);

			Alt.OnClient<RPPlayer>("Server:Warehouse:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:Warehouse:GiveKey", GiveKey);
			Alt.OnClient<RPPlayer>("Server:Warehouse:Upgrader", OpenUpgrader);
			Alt.OnClient<RPPlayer, int>("Server:Warehouse:Buy", BuyWarehouse);
			Alt.OnClient<RPPlayer, int>("Server:Warehouse:UpgradeBox", UpgradeWarehouseBoxes);
			Alt.OnClient<RPPlayer, int>("Server:Warehouse:Upgrade", UpgradeWarehouse);
		}

		private static void Open(RPPlayer player)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.WAREHOUSE);
			if (shape == null) return;

			var warehouse = WarehouseService.Get(shape.ShapeId);
			if (warehouse == null) return;

			player.ShowNativeMenu(true, new($"Lagerhalle {warehouse.Id}", new()
			{
				new($"Lagerhalle kaufen (${WarehouseController.WarehouseBuyPrice})", true, "Server:Warehouse:Buy", warehouse.Id)
			}));
		}

		private static void GiveKey(RPPlayer player, int targetId)
		{
			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			var warehouse = WarehouseService.GetByOwner(player.DbId, OwnerType.PLAYER);
			if (warehouse == null) return;

			if(warehouse.KeyHolderId > 0)
			{
				player.Notify("Information", "Du hast bereits einen Schlüssel vergeben!", NotificationType.ERROR);
				return;
			}

			var jumppoint = JumppointService.Get(warehouse.JumppointId);
			if (jumppoint == null) return;

			jumppoint.KeyHolderId = target.DbId;
			JumppointService.Update(jumppoint);

			warehouse.KeyHolderId = target.DbId;
			WarehouseService.Update(warehouse);

			var inventories = RPShape.All.Where(x => x.ShapeType == ColshapeType.WAREHOUSE_BOX && x.ShapeId == warehouse.Id).ToList();
			foreach (var inventory in inventories)
				inventory.InventoryAccess = new()
				{
					(player.DbId, OwnerType.PLAYER),
					(target.DbId, OwnerType.PLAYER)
				};

			player.Notify("Information", $"Du hast {target.Name} einen Schlüssel für deine Lagerhalle gegeben!", NotificationType.SUCCESS);
			target.Notify("Information", $"Du hast einen Schlüssel für Lagerhalle {warehouse.Id} erhalten!", NotificationType.SUCCESS);
		}

		private static void BuyWarehouse(RPPlayer player, int warehouseId)
		{
			var warehouse = WarehouseService.Get(warehouseId);
			if (warehouse == null || warehouse.OwnerId > 0) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(account.BankMoney < WarehouseController.WarehouseBuyPrice)
			{
				player.Notify("Information", "Du hast nicht genug Geld auf deinem Konto!", NotificationType.ERROR);
				return;
			}

			if(WarehouseService.HasWarehouse(player.DbId, OwnerType.PLAYER))
			{
				player.Notify("Information", "Du besitzt bereits eine Lagerhalle!", NotificationType.ERROR);
				return;
			}

			account.BankMoney -= WarehouseController.WarehouseBuyPrice;
			AccountService.Update(account);
			BankService.AddHistory(new(account.Id, account.Name, $"Lagerhalle {warehouse.Id}", TransactionType.PLAYER, true, WarehouseController.WarehouseBuyPrice, DateTime.Now));

			var shape = RPShape.All.FirstOrDefault(x => x.ShapeId == warehouseId && x.ShapeType == ColshapeType.WAREHOUSE);
			if (shape == null) return;

			RPShape.All.Remove(shape);
			shape.Destroy();

			var jumppoint = new JumppointModel($"Lagerhalle {warehouse.Id}", warehouse.PositionId, 0, WarehouseController.GetExitPosition(warehouse.Type), warehouse.Id, player.DbId, 0, OwnerType.PLAYER, true, DateTime.Now.AddYears(-1), JumppointType.WAREHOUSE);
			JumppointService.Add(jumppoint);
			JumppointController.LoadJumppoint(jumppoint);

			warehouse.OwnerId = player.DbId;
			warehouse.OwnerType = OwnerType.PLAYER;
			warehouse.JumppointId = jumppoint.Id;
			WarehouseService.Update(warehouse);
			player.Notify("Information", "Du hast eine Lagerhalle gekauft!", NotificationType.SUCCESS);
		}

		private static void OpenUpgrader(RPPlayer player)
		{
			if (player.Dimension < 1) return;

			var warehouse = WarehouseService.Get(player.Dimension);
			if (warehouse == null || !WarehouseController.IsWarehouseOwner(player, warehouse.OwnerId, warehouse.OwnerType)) return;

			player.ShowNativeMenu(true, new($"Lagerhalle {warehouse.Id}", new()
			{
				new("Kiste einbauen", true, "Server:Warehouse:UpgradeBox", warehouse.Id),
				new("Lagerhalle ausbauen", true, "Server:Warehouse:Upgrade", warehouse.Id)
			}));
		}

		private static void UpgradeWarehouseBoxes(RPPlayer player, int warehouseId)
		{
			var model = WarehouseService.Get(warehouseId);
			if (model == null || !WarehouseController.IsWarehouseOwner(player, model.OwnerId, model.OwnerType)) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if (model.OwnerType == OwnerType.TEAM && !account.TeamAdmin)
			{
				player.Notify("Information", "Nur Leader einer Fraktion können die Lagerhalle verbessern!", NotificationType.ERROR);
				return;
			}

			var inventories = WarehouseService.GetFromWarehouse(model.Id);
			var positions = WarehouseController.GetPositions(model.Type);

			if (inventories.Count >= positions.Count)
			{
				player.Notify("Information", "Die Lagerhalle ist bereits voll ausgebaut!", NotificationType.ERROR);
				return;
			}

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var items = InventoryService.GetInventoryItems(player.InventoryId);
			var boxItem = items.FirstOrDefault(x => x.ItemId == 28);
			if(boxItem == null || boxItem.Amount < 1)
			{
				player.Notify("Information", "Du benötigst eine Lagerkiste!", NotificationType.ERROR);
				return;
			}

			InventoryController.RemoveItem(inventory, boxItem.Slot, 1);

			var box = new InventoryModel(WarehouseController.WarehouseBoxSlots, WarehouseController.WarehouseBoxWeight, InventoryType.WAREHOUSE);
			InventoryService.Add(box);

			var warehouseInv = new WarehouseInventoryModel(model.Id, box.Id);
			WarehouseService.AddInventory(warehouseInv);

			WarehouseController.LoadWarehouseInventory(model, warehouseInv, positions[inventories.Count]);
			player.Notify("Information", "Du hast eine Lagerkiste eingebaut!", NotificationType.SUCCESS);
		}

		private static void UpgradeWarehouse(RPPlayer player, int warehouseId)
		{
			var model = WarehouseService.Get(warehouseId);
			if (model == null || !WarehouseController.IsWarehouseOwner(player, model.OwnerId, model.OwnerType)) return;

			if(model.Type >= WarehouseType.LARGE)
			{
				player.Notify("Information", "Deine Lagerhalle ist bereits auf der höchsten Stufe!", NotificationType.ERROR);
				return;
			}

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if (model.OwnerType == OwnerType.TEAM && !account.TeamAdmin)
			{
				player.Notify("Information", "Nur Leader einer Fraktion können die Lagerhalle verbessern!", NotificationType.ERROR);
				return;
			}

			if (account.Money < WarehouseController.WarehouseUpgradePrice)
			{
				player.Notify("Information", "Du hast nicht genug Geld dabei!", NotificationType.ERROR);
				return;
			}

			var outsidePos = PositionService.Get(model.PositionId);
			if (outsidePos == null) return;

			player.SetPosition(outsidePos.Position);
			player.Dimension = 0;

			foreach(var shape in RPShape.All.ToList())
			{
				if(shape.ShapeType == ColshapeType.WAREHOUSE_UPGRADE && shape.ShapeId == model.Id)
				{
					RPShape.All.Remove(shape);
					shape.Destroy();
					continue;
				}

				if(shape.ShapeType == ColshapeType.WAREHOUSE_BOX && shape.ShapeId == model.Id)
				{
					shape.Object?.Destroy();
					RPShape.All.Remove(shape);
					shape.Destroy();
					continue;
				}
			}

			var jumppoint = JumppointService.Get(model.JumppointId);
			if (jumppoint == null) return;

			var exitPos = PositionService.Get(WarehouseController.GetExitPosition(model.Type + 1));
			if (exitPos == null) return;

			var exitShape = RPShape.All.FirstOrDefault(x => x.ShapeType == ColshapeType.JUMP_POINT && x.ShapeId == jumppoint.Id && !x.JumppointEnterType);
			if (exitShape == null) return;

			exitShape.Position = exitPos.Position;

			model.Type++;
			jumppoint.InsidePositionId = exitPos.Id;
			JumppointService.Update(jumppoint);
			WarehouseService.Update(model);
			PlayerController.RemoveMoney(player, WarehouseController.WarehouseUpgradePrice);
			WarehouseController.LoadWarehouse(model);
			player.Notify("Information", "Du hast deine Lagerhalle aufgerüstet!", NotificationType.SUCCESS);
		}
	}
}