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
		private static List<float> TeamDealerTake = new()
		{
			0.26f, // R0
			0.24f, // R1
			0.22f, // R2
			0.20f, // R3
			0.18f, // R4
			0.16f, // R5
			0.14f, // R6
			0.12f, // R7
			0.10f, // R8
			0.08f, // R9
			0.06f, // R10
			0.04f, // R11
			0.02f, // R12
		};

		[Initialize]
		public static void Initialize()
		{
			var models = DealerService.GetAll();

			DealerController.ResetDealers(models);

			foreach (var model in models)
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

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = DealerService.GetItem(itemId);
			if (item == null) return;

			if (InventoryService.HasItems(player.InventoryId, item.ItemId) < 1)
			{
				player.ShowNativeMenu(false, new());
				player.Notify("Information", "Du hast dieses Item nicht dabei!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var itemBase = InventoryService.GetItem(item.ItemId);
			if (itemBase == null) return;

			var price = item.Price;

			if(item.ItemId == 3 || item.ItemId == 276)
			{
				var team = TeamService.Get(player.TeamId);
				if (team != null)
				{
					var percentage = TeamDealerTake.Count <= account.TeamRank ? 0 : TeamDealerTake[account.TeamRank];
					var take = (int)Math.Round(price * percentage);

					team.Money += take;
					TeamService.Update(team);

					price -= take;
					player.Notify("Information", $"Du hast ein Item an den Dealer verkauft! Davon gingen ${take} an die Fraktion.", Core.Enums.NotificationType.SUCCESS);
				}
			}
			else
			{
				player.Notify("Information", $"Du hast ein Item an den Dealer verkauft!", Core.Enums.NotificationType.SUCCESS);
			}

			InventoryController.RemoveItem(inventory, itemBase, 1);
			PlayerController.AddMoney(player, item.Price);
		}
	}
}
