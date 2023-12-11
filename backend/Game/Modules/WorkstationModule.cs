using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Models.Workstation;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules
{
	public static class WorkstationModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in WorkstationService.GetAll())
				WorkstationController.LoadWorkstation(model);

			Alt.OnClient<RPPlayer, int, int>("Server:Workstation:Take", Take);
			Alt.OnClient<RPPlayer, int, int>("Server:Workstation:Start", Start);
			Alt.OnClient<RPPlayer, int>("Server:Workstation:Open", Open);
		}

		private static void Take(RPPlayer player, int stationId, int itemId)
		{
			var station = WorkstationService.Get(stationId);
			if (station == null) return;

			var item = WorkstationService.GetItem(itemId);
			if (item == null || item.AccountId != player.DbId || item.WorkstationId != stationId || item.TimeLeft > 0) return;

			if(!InventoryController.AddItem(player.InventoryId, item.ItemId, item.ItemAmount)) return;
			WorkstationService.RemoveItem(item);

			player.Notify("Information", $"Du hast einen Auftrag abgeholt!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void Start(RPPlayer player, int stationId, int bpId)
		{
			var station = WorkstationService.Get(stationId);
			if (station == null) return;

			var bp = WorkstationService.GetBlueprint(bpId);
			if (bp == null) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var itemsCount = WorkstationService.GetItemsCount(account.Id, station.Id, bp.ItemId);
			if(itemsCount >= bp.Max)
			{
				player.Notify("Information", $"Du hast bereits {bp.Max} Aufträge dieser Art!", Core.Enums.NotificationType.ERROR);
				return;
			}

			if(account.Money < bp.Price)
			{
				player.Notify("Information", $"Du benötigst ${bp.Price} für diesen Auftrag!", Core.Enums.NotificationType.ERROR);
				return;
			}

			if(InventoryService.HasItems(player.InventoryId, bp.NeededItem) < bp.NeededItemAmount)
			{
				player.Notify("Information", $"Du hast nicht alle nötigen Materialien dabei!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = InventoryService.GetItem(bp.NeededItem);
			if (item == null) return;

			if (!InventoryController.RemoveItem(inventory, item, bp.NeededItemAmount))
			{
				player.Notify("Information", $"Es ist ein Fehler aufgetreten!", Core.Enums.NotificationType.ERROR);
				return;
			}

			PlayerController.RemoveMoney(player, bp.Price);
			WorkstationService.AddItem(new(player.DbId, station.Id, bp.ItemId, bp.ItemAmount, DateTime.Now, bp.Duration));
			player.Notify("Information", $"Du hast einen Auftrag gestartet!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void Open(RPPlayer player, int stationId)
		{
			var station = WorkstationService.Get(stationId);
			if (station == null) return;

			var bps = WorkstationService.GetBlueprints(stationId);
			var items = WorkstationService.GetItems(player.DbId, stationId);

			player.ShowComponent("Workstation", true, JsonConvert.SerializeObject(new
			{
				Id = station.Id,
				Items = ConvertBlueprints(bps),
				Running = ConvertItems(items)
			}));
		}

		private static List<object> ConvertBlueprints(List<WorkstationBlueprintModel> bps)
		{
			var result = new List<object>();
			var items = InventoryService.GetItems();

			foreach (var model in bps)
			{
				var gainItem = items.FirstOrDefault(x => x.Id == model.ItemId);
				var neededItem = items.FirstOrDefault(x => x.Id == model.NeededItem);
				if (gainItem == null || neededItem == null) continue;

				result.Add(new
				{
					Id = model.Id,
					Input = neededItem.Name,
					InputAmount = model.NeededItemAmount,
					Output = gainItem.Name,
					OutputAmount = model.ItemAmount,
					Price = model.Price,
					Duration = model.Duration,
					Max = model.Max
				});
			}

			return result;
		}

		private static List<object> ConvertItems(List<WorkstationItemModel> args)
		{
			var result = new List<object>();
			var items = InventoryService.GetItems();

			foreach (var model in args)
			{
				var item = items.FirstOrDefault(x => x.Id == model.ItemId);
				if (item == null) continue;

				result.Add(new
				{
					Id = model.Id,
					Output = item.Name,
					OutputAmount = model.ItemAmount,
					Started = model.Added.ToString("HH:mm dd.mm.yyyy"),
					TimeLeft = model.TimeLeft
				});
			}

			return result;
		}

		[EveryMinute]
		public static void Tick()
		{
			var accounts = AccountService.GetAllIdOnly();
			var workstations = WorkstationService.GetAll();
			var items = WorkstationService.GetAllItems();

			foreach (var station in workstations)
			{
				foreach(var account in  accounts)
				{
					var accountItems = items.Where(x => x.WorkstationId == station.Id && x.AccountId == account);
					var item = accountItems.FirstOrDefault(x => x.TimeLeft > 0);
					if (item == null) continue;

					item.TimeLeft--;
				}
			}

			WorkstationService.UpdateItems(items);
		}
	}
}