using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;
using Core.Enums;
using Database.Models.Wardrobe;
using Database.Models.ClothesShop;

namespace Game.Modules
{
    public static class ClothesShopModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in ClothesShopService.GetAll())
				ClothesShopController.LoadClothesShop(model);

			Alt.OnClient<RPPlayer, int>("Server:ClothesShop:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:ClothesShop:Try", Try);
			Alt.OnClient<RPPlayer>("Server:ClothesShop:ResetClothes", Reset);
			Alt.OnClient<RPPlayer, int, string>("Server:ClothesShop:Buy", Buy);
		}

		private static void Open(RPPlayer player, int id)
		{
			if (player.AdminDuty) return;

			var shop = ClothesShopService.Get(id);
			if (shop == null) return;

			var pos = PositionService.Get(shop.PositionId);
			if (pos == null) return;

			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			var items = ClothesShopService.GetItemsFromShop(id, custom.Gender ? 1 : 0);
			if (items == null) return;

			player.Rotation = pos.Rotation;
			player.ShowComponent("ClothesShop", true, JsonConvert.SerializeObject(new
			{
				Id = shop.Id,
				Items = items,
				Type = shop.Type
			}));
		}

		private static void Try(RPPlayer player, int id)
		{
			var clothes = ClothesShopService.GetItem(id);
			if (clothes == null) return;

			if (clothes.IsProp) player.SetProp(clothes.Component, clothes.Drawable, clothes.Texture, clothes.Dlc);
			else player.SetClothing(clothes.Component, clothes.Drawable, clothes.Texture, clothes.Dlc);
		}

		private static void Reset(RPPlayer player)
		{
			PlayerController.ApplyPlayerClothes(player);
		}

		private static void Buy(RPPlayer player, int id, string data)
		{
			var shop = ClothesShopService.Get(id);
			if (shop == null) return;

			var pos = PositionService.Get(shop.PositionId);
			if (pos == null || player.Position.Distance(pos.Position) > 5f) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			var ids = JsonConvert.DeserializeObject<List<int>>(data);
			if (ids == null) return;

			var wardrobeItems = WardrobeService.GetItemsFromOwner(player.DbId, player.TeamId, custom.Gender ? 1 : 0);
			var items = ClothesShopService.GetItemsFromShop(id, custom.Gender ? 1 : 0);
			var newClothes = new List<ClothesShopItemModel>();
			var ownedClothes = new List<ClothesShopItemModel>();

			foreach (var itemId in ids)
			{
				var item = items.FirstOrDefault(x => x.Id == itemId);
				if (item == null) continue;

				if(wardrobeItems.Any(x => x.Component == item.Component && x.Drawable == item.Drawable && x.Texture == item.Texture && x.Dlc == item.Dlc))
				{
					ownedClothes.Add(item);
					continue;
				}

				newClothes.Add(item);
			}

			var price = newClothes.Sum(x => x.Price);

			if (account.Money < price)
			{
				player.Notify("Kleidungsladen", "Du hast nicht genug Geld dabei!", NotificationType.ERROR);
				return;
			}

			var newWardrobeItems = new List<WardrobeItemModel>();

			var clothes = ClothesService.Get(player.ClothesId);
			if (clothes == null) return;

			PlayerController.RemoveMoney(player, price);

			foreach (var item in newClothes)
			{
				newWardrobeItems.Add(new(player.DbId, OwnerType.PLAYER, item.Label, custom.Gender ? 1 : 0, item.Component, item.Drawable, item.Texture, item.Dlc, item.IsProp));

				if (item.IsProp) PlayerController.SetProps(player, clothes, item.Component, item.Drawable, item.Texture, item.Dlc);
				else PlayerController.SetClothes(player, clothes, item.Component, item.Drawable, item.Texture, item.Dlc);
			}

			foreach(var item in ownedClothes)
			{
				if (item.IsProp) PlayerController.SetProps(player, clothes, item.Component, item.Drawable, item.Texture, item.Dlc);
				else PlayerController.SetClothes(player, clothes, item.Component, item.Drawable, item.Texture, item.Dlc);
			}

			WardrobeService.AddItems(newWardrobeItems);
			ClothesService.Update(clothes);
			PlayerController.ApplyPlayerClothes(player);
			player.Notify("Kleidungsladen", $"Du hast {ids.Count} Kleidungsstücke für ${price} erworben", NotificationType.SUCCESS);
		}
	}
}