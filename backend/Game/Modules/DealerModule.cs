using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;

namespace Game.Modules
{
	public static class DealerModule
	{
		private static List<float> TeamDealerTake = new()
		{
			0.12f, // R0
			0.11f, // R1
			0.10f, // R2
			0.09f, // R3
			0.08f, // R4
			0.07f, // R5
			0.06f, // R6
			0.05f, // R7
			0.04f, // R8
			0.03f, // R9
			0.02f, // R10
			0.01f, // R11
			0.00f, // R12
		};

		private static int MinuteTicks = 0;
		private static Random Random = new();
		private static DateTime LastSnitch;

		[Initialize]
		public static void Initialize()
		{
			var items = DealerService.GetAllItems();
			var models = DealerService.GetAll();

			DealerController.ResetDealers(models);

			for (var i = 0; i < 4; i++)
				DealerController.LoadDealer(models[new Random().Next(0, models.Count)], items);

			Alt.OnClient<RPPlayer>("Server:Dealer:Open", Open);
			Alt.OnClient<RPPlayer, int, int>("Server:Dealer:Sell", Sell);
		}

		private static void Open(RPPlayer player)
		{
			if (player.TeamId <= 5) return;

			var shape = RPShape.Get(player.Position, player.Dimension, Core.Enums.ColshapeType.DEALER);
			if (shape == null) return;

			var dealerCache = DealerController.DealerCache.FirstOrDefault(x => x.DealerId == shape.ShapeId);
			if (dealerCache == null) return;

			var itemBases = InventoryService.GetItems();
			var items = DealerService.GetAllItems();
			var nativeItems = new List<NativeMenuItem>();

			foreach(var item in items)
			{
				var itemBase = itemBases.FirstOrDefault(x => x.Id == item.ItemId);
				if (itemBase == null) continue;

				var itemCache = dealerCache.Items.FirstOrDefault(x => x.Id == item.Id);
				if (itemCache == null) continue;

				nativeItems.Add(new($"{itemBase.Name} - ${itemCache.Price}", false, "Server:Dealer:Sell", shape.ShapeId, item.Id));
			}

			player.ShowNativeMenu(true, new("Dealer", nativeItems));
		}

		private static void Sell(RPPlayer player, int dealerId, int itemId)
		{
			var shape = RPShape.All.FirstOrDefault(x => x.ShapeId == dealerId && x.ShapeType == Core.Enums.ColshapeType.DEALER);
			if(shape == null || shape.Position.Distance(player.Position) > shape.Size)
			{
				player.ShowNativeMenu(false, new());
				return;
			}

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = DealerService.GetItem(itemId);
			if (item == null) return;

			var itemCount = InventoryService.HasItems(player.InventoryId, item.ItemId);

            if (itemCount < 1)
			{
				player.ShowNativeMenu(false, new());
				player.Notify("Information", "Du hast dieses Item nicht dabei!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var dealerCache = DealerController.DealerCache.FirstOrDefault(x => x.DealerId == shape.ShapeId);
			if (dealerCache == null) return;

			var itemCache = dealerCache.Items.FirstOrDefault(x => x.Id == itemId);
			if (itemCache == null) return;

			if(itemCache.SellCap > 0)
			{
				if(itemCache.ItemsSold >= itemCache.SellCap)
				{
                    player.Notify("Information", "Der Dealer kann aktuell nichts mehr ankaufen!", Core.Enums.NotificationType.ERROR);
                    return;
                }

				if(itemCache.ItemsSold + itemCount > itemCache.SellCap)
				{
					itemCount = itemCache.SellCap - itemCache.ItemsSold;
                }
			}

			var itemBase = InventoryService.GetItem(item.ItemId);
			if (itemBase == null) return;

			var price = itemCache.Price * itemCount;

			itemCache.ItemsSold += itemCount;

            // Todo: Update Items
            if (item.ItemId == 3 || item.ItemId == 276)
			{
				var now = DateTime.Now;
				if(now.Hour > 18)
				{
					price += (int)Math.Round(price * 0.15);
                    player.Notify("Information", $"Du hast einen 15% Risiko-Bonus erhalten!", Core.Enums.NotificationType.INFO);
                }

				var team = TeamService.Get(player.TeamId);
				if (team != null)
				{
					var percentage = TeamDealerTake.Count <= account.TeamRank ? 0 : TeamDealerTake[account.TeamRank];
					var take = (int)Math.Round(price * percentage);

					team.Money += take;
					TeamService.Update(team);

					price -= take;
					player.Notify("Information", $"Du hast {itemCount}x Items an den Dealer verkauft! Davon gingen ${take} an die Fraktion.", Core.Enums.NotificationType.SUCCESS);
				}

				if(Random.Next(100) < 5 && LastSnitch.AddHours(2) < DateTime.Now)
				{
					TeamController.Broadcast(2, "Ein Informant hat einen Drogenhandel gemeldet!", Core.Enums.NotificationType.WARN);
					var blip = (RPBlip)Alt.CreateBlip(false, BlipType.Radius, player.Position + new Position(Random.Next(-70, 70), Random.Next(-70, 70), 0), RPPlayer.All.Where(x => x.TeamId == 2).ToArray());
					blip.DeleteAt = DateTime.Now.AddMinutes(30);
					blip.Sprite = 4;
					blip.ScaleXY = new(300, 300);
                    blip.ShortRange = true;
                }
			}
			else
			{
				player.Notify("Information", $"Du hast {itemCount}x Items an den Dealer verkauft!", Core.Enums.NotificationType.SUCCESS);
			}

			InventoryController.RemoveItem(inventory, itemBase, itemCount);
			PlayerController.AddMoney(player, price);
		}

        [EveryMinute]
        public static void EveryMinute()
        {
			MinuteTicks++;

			if(MinuteTicks >= 5)
			{
				MinuteTicks = 0;

				foreach(var dealer in DealerController.DealerCache)
				{
					foreach(var item in dealer.Items)
					{
						item.ItemsSold = 0;
					}
				}
			}
        }
    }
}
