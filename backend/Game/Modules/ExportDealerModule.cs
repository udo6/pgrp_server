using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class ExportDealerModule
	{
		private static Position Position = new(797.578f, -2988.7385f, 5.010254f);

		[Initialize]
		public static void Initialize()
		{
			var shape = (RPShape)Alt.CreateColShapeCylinder(Position, 2f, 2f);
			shape.ShapeId = 1;
			shape.ShapeType = ColshapeType.EXPORT_DEALER;
			shape.Size = 2f;

			var blip = Alt.CreateBlip(true, 4, Position, Array.Empty<IPlayer>());
			blip.Name = "Exporthandel";
			blip.Sprite = 478;
			blip.Color = 4;
			blip.ShortRange = true;

			Alt.OnClient<RPPlayer>("Server:ExportDealer:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:ExportDealer:Sell", Sell);
		}

		private static void Open(RPPlayer player)
		{
			var items = InventoryService.GetItems();
			var exportItems = ExportDealerService.GetAll();
			var nativeItems = new List<NativeMenuItem>();
			foreach(var exportItem in exportItems)
			{
				var item = items.FirstOrDefault(x => x.Id == exportItem.ItemId);
				if (item == null) continue;

				nativeItems.Add(new($"{item.Name} - ${exportItem.Price}", true, "Server:ExportDealer:Sell", exportItem.Id, item.Id));
			}

			player.ShowNativeMenu(true, new("Exporthändler", nativeItems));
		}

		private static void Sell(RPPlayer player, int exportItemId)
		{
			var exportItem = ExportDealerService.Get(exportItemId);
			if (exportItem == null) return;

			var itemsCount = InventoryService.HasItems(player.InventoryId, exportItem.ItemId);
			if (itemsCount < 1) return;

			var item = InventoryService.GetItem(exportItem.ItemId);
			if (item == null) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var price = itemsCount * exportItem.Price;
			InventoryController.RemoveItem(inventory, item, itemsCount);
			PlayerController.AddMoney(player, price);
			player.Notify("Information", $"Du hast {itemsCount} Items für ${price} verkauft!", NotificationType.SUCCESS);
		}
	}
}