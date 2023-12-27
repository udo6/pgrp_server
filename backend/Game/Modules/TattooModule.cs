using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules
{
	public static class TattooModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in TattooService.GetAllShops())
				TattooController.LoadTattooShop(model);

			Alt.OnClient<RPPlayer, int>("Server:Tattoo:Open", Open);
			Alt.OnClient<RPPlayer, uint, uint>("Server:Tattoo:Try", Try);
			Alt.OnClient<RPPlayer>("Server:Tattoo:Reset", Reset);
			Alt.OnClient<RPPlayer, int>("Server:Tattoo:Buy", Buy);
		}

		private static void Open(RPPlayer player, int shopId)
		{
			var items = TattooService.GetItemsFromShop(shopId);

			player.ShowComponent("Tattoo", true, JsonConvert.SerializeObject(items));
		}

		private static void Try(RPPlayer player, uint collection, uint overlay)
		{
			player.RemoveDecoration(player.TemporaryTattoo.Item1, player.TemporaryTattoo.Item2);
			player.TemporaryTattoo = (collection, overlay);
			player.AddDecoration(collection, overlay);
		}

		private static void Reset(RPPlayer player)
		{
			PlayerController.ApplyPlayerTattoos(player);
		}

		private static void Buy(RPPlayer player, int itemId)
		{
			var item = TattooService.GetItem(itemId);
			if (item == null) return;

			if(TattooService.HasItem(player.DbId, item.Collection, item.Overlay))
			{
				player.Notify("Information", "Du hast dieses Tattoo bereits!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(account.Money < item.Price)
			{
				player.Notify("Information", "Du hast nicht genug Geld dabei!", Core.Enums.NotificationType.ERROR);
				return;
			}

			PlayerController.RemoveMoney(player, item.Price);
			TattooService.Add(new(player.DbId, item.Collection, item.Overlay));
			player.Notify("Information", "Du hast dir ein Tattoo stechen lassen!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}