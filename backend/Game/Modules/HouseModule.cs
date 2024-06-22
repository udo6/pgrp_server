using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Core.Enums;
using Database.Models.Jumpoint;

namespace Game.Modules
{
	public static class HouseModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var house in HouseService.GetAll())
				HouseController.LoadHouse(house);

			Alt.OnClient<RPPlayer>("Server:House:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:House:GiveKey", GiveKey);
			Alt.OnClient<RPPlayer, int>("Server:House:Buy", Buy);
			Alt.OnClient<RPPlayer>("Server:House:Sell", Sell);
		}

		private static void Open(RPPlayer player)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.HOUSE);
			if (shape == null) return;

			var house = HouseService.Get(shape.ShapeId);
			if (house == null) return;

			player.ShowNativeMenu(true, new($"Haus {house.Id}", new()
			{
				new($"Haus kaufen (${HouseController.HousePrices[house.Type]})", true, "Server:House:Buy", house.Id)
			}));
		}

		private static void GiveKey(RPPlayer player, int targetId)
		{
			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			var house = HouseService.GetByOwner(player.DbId);
			if (house == null) return;

			if (house.KeyHolderId > 0)
			{
				player.Notify("Information", "Du hast bereits einen Schlüssel vergeben!", NotificationType.ERROR);
				return;
			}

			var jumppoint = JumppointService.Get(house.JumppointId);
			if (jumppoint == null) return;

			jumppoint.KeyHolderId = target.DbId;
			JumppointService.Update(jumppoint);

			house.KeyHolderId = target.DbId;
			HouseService.Update(house);

			var houseInv = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == ColshapeType.HOUSE_INVENTORY && x.ShapeId == house.Id);
			if(houseInv != null)
			{
				houseInv.InventoryAccess = new()
				{
					(player.DbId, OwnerType.PLAYER),
					(target.DbId, OwnerType.PLAYER)
				};
			}

			player.Notify("Information", $"Du hast {target.Name} einen Schlüssel für dein Haus gegeben!", NotificationType.SUCCESS);
			target.Notify("Information", $"Du hast einen Schlüssel für Haus {house.Id} erhalten!", NotificationType.SUCCESS);
		}

		private static void Buy(RPPlayer player, int houseId)
		{
			var house = HouseService.Get(houseId);
			if (house == null || house.OwnerId > 0) return;

			if (HouseService.HasPlayerHouse(player.DbId))
			{
				player.Notify("Information", "Du hast bereits ein Haus!", NotificationType.ERROR);
				return;
			}

			var price = HouseController.HousePrices[house.Type];

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(account.BankMoney < price)
			{
				player.Notify("Information", "Du hast nicht genug Geld auf deinem Konto!", NotificationType.ERROR);
				return;
			}

			var shape = RPShape.All.FirstOrDefault(x => x.ShapeId == houseId && x.ShapeType == ColshapeType.HOUSE);
			if (shape == null) return;

			RPShape.All.Remove(shape);
			shape.Destroy();

			account.BankMoney -= price;
			AccountService.Update(account);
			BankService.AddHistory(new(account.Id, account.Name, $"Haus {houseId}", TransactionType.PLAYER, true, price, DateTime.Now));

			var jumppoint = new JumppointModel(
				$"Haus {houseId}",
				house.PositionId,
				0,
				HouseController.HousePositions[house.Type],
				house.Id,
				player.DbId,
				0,
				OwnerType.PLAYER,
				true,
				DateTime.Now.AddYears(-1),
				JumppointType.HOUSE);
			JumppointService.Add(jumppoint);
			JumppointController.LoadJumppoint(jumppoint);

			house.OwnerId = player.DbId;
			house.JumppointId = jumppoint.Id;
			HouseService.Update(house);

			player.Notify("Information", $"Du hast Haus {houseId} für ${price} gekauft!", NotificationType.SUCCESS);
		}

		private static void Sell(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var house = HouseService.GetByOwner(player.DbId);
			if (house == null) return;

			var jumppoint = JumppointService.Get(house.JumppointId);
			if (jumppoint == null) return;

			var price = (int)(HouseController.HousePrices[house.Type] * 0.75);

			JumppointService.Remove(jumppoint);

			account.BankMoney += price;
			AccountService.Update(account);
			BankService.AddHistory(new(account.Id, account.Name, $"Haus {house.Id}", TransactionType.PLAYER, false, price, DateTime.Now));

			house.OwnerId = 0;
			house.JumppointId = 0;
			HouseService.Update(house);

			player.Notify("Information", $"Du hast dein Haus für ${price} an den Staat verkauft!", NotificationType.SUCCESS);
		}
	}
}