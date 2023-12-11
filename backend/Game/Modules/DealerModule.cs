using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class DealerModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in DealerService.GetAll())
				DealerController.LoadDealer(model);

			Alt.OnClient<RPPlayer>("Server:Dealer:Open", Open);
			Alt.OnClient<RPPlayer, int, int>("Server:Dealer:Sell", Sell);
		}

		private static void Open(RPPlayer player)
		{
			if (player.TeamId <= 5) return;

			var shape = RPShape.All.FirstOrDefault(x => x.ShapeType == Core.Enums.ColshapeType.DEALER && x.Dimension == player.Dimension && x.Position.Distance(player.Position) <= x.Size);
			if (shape == null) return;

			var itemBases = InventoryService.GetItems();
			var items = DealerService.GetAllItems();
			var nativeItems = new List<NativeMenuItem>();

			foreach(var item in items)
			{
				var itemBase = itemBases.FirstOrDefault(x => x.Id == item.ItemId);
				if (itemBase == null) continue;

				nativeItems.Add(new($"{itemBase.Name} - ${item.Price}", false, "Server:Dealer:Sell", shape.Id, item.Id));
			}

			player.ShowNativeMenu(true, new("Dealer", nativeItems));
		}

		private static void Sell(RPPlayer player, int dealerId, int itemId)
		{
			var shape = RPShape.All.FirstOrDefault(x => x.Id == dealerId && x.ShapeType == Core.Enums.ColshapeType.DEALER);
			if(shape == null || shape.Position.Distance(player.Position) > shape.Size)
			{
				player.ShowNativeMenu(false, new());
				return;
			}

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = DealerService.GetItem(itemId);
			if (item == null) return;

			var itemBase = InventoryService.GetItem(item.ItemId);
			if (itemBase == null) return;

			InventoryController.RemoveItem(inventory, itemBase, 1);
			PlayerController.AddMoney(player, item.Price);
		}
	}
}
