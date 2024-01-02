using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models.Shop;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules
{
    public static class ShopModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var shop in ShopService.GetAll())
				ShopController.LoadShop(shop);

			ShopController.ResetItemPrices();

			Alt.OnClient<RPPlayer, int>("Server:Shop:Open", Open);
			Alt.OnClient<RPPlayer, int, string>("Server:Shop:BuyItems", BuyItems);
		}

		private static void Open(RPPlayer player, int shopId)
		{
			var shop = ShopService.Get(shopId);
			if (shop == null || !HasPermission(player, shop.OwnerId, shop.Type)) return;

			var items = ShopService.GetShopItems(shopId);

			player.ShowComponent("Shop", true, JsonConvert.SerializeObject(new
			{
				Id = shop.Id,
				Name = shop.Name,
				Items = GetData(items, shop.OwnerId > 0)
			}));
		}

		private static void BuyItems(RPPlayer player, int shopId, string json)
		{
			player.ShowComponent("Shop", false);

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var shop = ShopService.Get(shopId);
			if (shop == null) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var inventoryItems = InventoryService.GetInventoryItems(inventory.Id);
			var shopItems = ShopService.GetShopItems(shopId);
			var itemBases = InventoryService.GetItems();

			var data = JsonConvert.DeserializeObject<List<ShopData>>(json);
			if (data == null) return;

			var price = 0;
			var weight = 0f;
			var slots = 0;

			foreach(var item in data)
			{
				if (item.Amount < 1 || item.Amount > 100) return;

				var shopItem = shopItems.FirstOrDefault(x => x.Id == item.Id);
				if (shopItem == null) continue;

				var itemBase = itemBases.FirstOrDefault(x => x.Id == shopItem.ItemId);
				if (itemBase == null) continue;

				if (shopItem.MinRank > account.TeamRank)
				{
					player.Notify("Information", $"Du kannst {itemBase.Name} erst ab Rang {shopItem.MinRank} kaufen!", NotificationType.ERROR);
					return;
				}

				price += shopItem.Price * item.Amount;
				weight += itemBase.Weight * item.Amount;
				slots += (int)Math.Ceiling((double)item.Amount / itemBase.StackSize);
			}

			if(price > account.Money)
			{
				player.Notify("Information", "Du hast nicht genug Geld dabei!", NotificationType.ERROR);
				return;
			}

			if(InventoryController.CalcInventoryWeight(inventory) + weight > inventory.MaxWeight)
			{
				player.Notify("Information", "Du hast nicht genug Platz!", NotificationType.ERROR);
				return;
			}

			if (inventoryItems.Count + slots > inventory.Slots)
			{
				player.Notify("Information", "Du hast nicht genug Platz!", NotificationType.ERROR);
				return;
			}

			PlayerController.RemoveMoney(player, price);
			foreach(var item in data)
			{
				var shopItem = shopItems.FirstOrDefault(x => x.Id == item.Id);
				if (shopItem == null) continue;

				var itemBase = itemBases.FirstOrDefault(x => x.Id == shopItem.ItemId);
				if (itemBase == null) continue;

				InventoryController.AddItem(inventory, itemBase, item.Amount);
			}

			player.Notify("Information", $"Du hast {data.Count} Items für ${price} erworben!", NotificationType.SUCCESS);
		}

		private static List<object> GetData(List<ShopItemModel> items, bool ranks)
		{
			var itemBases = InventoryService.GetItems();
			var result = new List<object>();

			foreach (var item in items)
			{
				var itemBase = itemBases.FirstOrDefault(x => x.Id == item.ItemId);
				if (itemBase == null) continue;

				var name = ranks ? $"{itemBase.Name} (R{item.MinRank})" : itemBase.Name;

				result.Add(new
				{
					Id = item.Id,
					ItemId = itemBase.Id,
					ItemName = name,
					ItemIcon = itemBase.Icon,
					Price = item.Price
				});
			}

			return result;
		}

		private static bool HasPermission(RPPlayer player, int owner, ShopType type)
		{
			return type == ShopType.TWENTYFOURSEVEN ||
				type == ShopType.AMMUNATION ||
				type == ShopType.MECHANIC ||
				type == ShopType.FOOD ||
				type == ShopType.NO_BLIP ||
				(type == ShopType.TEAM && player.TeamId == owner) ||
				(type == ShopType.SWAT && player.SWATDuty) ||
				(type == ShopType.ALL_TEAMS_ONLY && player.TeamId > 5);
		}
	}

	public class ShopData
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		public string Name { get; set; }
		public int Amount { get; set; }

		public ShopData()
		{
			Name = string.Empty;
		}

		public ShopData(int id, int itemId, string name, int amount)
		{
			Id = id;
			ItemId = itemId;
			Name = name;
			Amount = amount;
		}
	}
}