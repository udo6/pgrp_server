using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Models;
using Database.Models.Inventory;
using Database.Models.Vehicle;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class VehicleShopModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in VehicleShopService.GetAll())
				VehicleShopController.LoadVehicleShop(model);

			Alt.OnClient<RPPlayer>("Server:VehicleShop:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:VehicleShop:Buy", Buy);
		}

		private static void Open(RPPlayer player)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.VEHICLE_SHOP);
			if (shape == null) return;

			var items = VehicleShopService.GetItemsFromShop(shape.ShapeId);
			var bases = VehicleService.GetAllBases();

			var nativeItems = new List<NativeMenuItem>();
			foreach(var item in items)
			{
				var baseModel = bases.FirstOrDefault(x => x.Id == item.VehicleBaseId);
				if (baseModel == null) continue;

				nativeItems.Add(new($"{baseModel.Name} - ${baseModel.Price}", true, "Server:VehicleShop:Buy", item.Id));
			}

			player.ShowNativeMenu(true, new("Fahrzeughändler", nativeItems));
		}

		private static void Buy(RPPlayer player, int itemId)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.VEHICLE_SHOP);
			if (shape == null) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var item = VehicleShopService.GetItem(itemId);
			if (item == null) return;

			var baseModel = VehicleService.GetBase(item.VehicleBaseId);
			if (baseModel == null) return;

			if(account.BankMoney < baseModel.Price)
			{
				player.Notify("Information", "Du hast nicht genug Geld auf deinem Bankkonto!", NotificationType.ERROR);
				return;
			}

			var shop = VehicleShopService.Get(item.ShopId);
			if (shop == null) return;

			var spawns = VehicleShopService.GetSpawnsFromShop(shop.Id);
			PositionModel? spawn = null;

			foreach(var _spawn in spawns)
			{
				var _pos = PositionService.Get(_spawn.PositionId);
				if (_pos == null) continue;

				if (RPVehicle.All.Any(x => x.Position.Distance(_pos.Position) < 2f)) continue;

				spawn = _pos;
			}

			if(spawn == null)
			{
				player.Notify("Information", "Es ist kein Spawn Punkt frei!", NotificationType.ERROR);
				return;
			}

			account.BankMoney -= baseModel.Price;
			AccountService.Update(account);
			BankService.AddHistory(new(account.Id, account.Name, $"Fahrzeugkauf", TransactionType.PLAYER, true, baseModel.Price, DateTime.Now));

			var pos = new PositionModel(spawn.Position, spawn.Rotation);
			PositionService.Add(pos);

			var trunk = new InventoryModel(baseModel.TrunkSlots, baseModel.TrunkWeight, InventoryType.TRUNK);
			var glovebox = new InventoryModel(baseModel.GloveBoxSlots, baseModel.GloveBoxWeight, InventoryType.GLOVEBOX);
			InventoryService.Add(trunk, glovebox);

			var tune = new TuningModel();
			TuningService.Add(tune);

			var veh = new VehicleModel(player.DbId, 0, false, "0", "", 100, OwnerType.PLAYER, 16, pos.Id, trunk.Id, glovebox.Id, item.VehicleBaseId, tune.Id);
			VehicleService.AddVehicle(veh);
			VehicleController.LoadVehicle(veh);
			player.Notify("Information", $"Du hast ein Fahrzeug für {baseModel.Price} gekauft!", NotificationType.SUCCESS);
		}
	}
}