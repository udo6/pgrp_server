using AltV.Net;
using AltV.Net.Data;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class PoliceModule
	{
		public static bool SWATStatus = false;
		public static Position ShopPosition = new(-1079.9077f, -823.05493f, 14.873291f);

		private static List<Position> TeleporterPositions = new()
		{
			new(-1096.3121f, -850.16705f, 4.8813477f),
			new(-1096.3121f, -850.16705f, 10.273315f),
			new(-1096.3121f, -850.16705f, 13.693726f),
			new(-1096.3121f, -850.16705f, 18.98462f),
			new(-1096.3121f, -850.16705f, 23.028564f),
			new(-1096.3121f, -850.16705f, 26.819824f),
			new(-1096.3121f, -850.16705f, 30.745728f),
			new(-1096.3121f, -850.16705f, 34.351685f),
			new(-1096.3121f, -850.16705f, 38.22705f)
		};

		[Initialize]
		public static void Initialize()
		{
			var shape = (RPShape)Alt.CreateColShapeCylinder(ShopPosition.Down(), 2f, 2f);
			shape.ShapeId = 1;
			shape.ShapeType = ColshapeType.SWAT_SHOP;
			shape.Size = 2f;

			var index = 0;
			foreach(var tp in TeleporterPositions)
			{
				var tpShape = (RPShape)Alt.CreateColShapeCylinder(tp.Down(), 2f, 2f);
				tpShape.ShapeId = index;
				tpShape.ShapeType = ColshapeType.LSPD_TELEPORTER;
				tpShape.Size = 2f;
				index++;
			}

			Alt.OnClient<RPPlayer, int>("Server:Police:Teleport", UseTeleporter);
			Alt.OnClient<RPPlayer>("Server:Police:OpenTeleporter", OpenTeleporter);
			Alt.OnClient<RPPlayer, int, int>("Server:Police:TakeLicense", TakeLicense);
			Alt.OnClient<RPPlayer>("Server:SWAT:OpenShop", OpenShop);
			Alt.OnClient<RPPlayer, bool>("Server:SWAT:SetSWATDuty", SetSWATDuty);
			Alt.OnClient<RPPlayer, int, int>("Server:SWAT:Buy", BuyItem);
		}

		private static void UseTeleporter(RPPlayer player, int index)
		{
			if (index >= TeleporterPositions.Count) return;

			var pos = TeleporterPositions[index];
			player.SetPosition(pos);
		}

		private static void OpenTeleporter(RPPlayer player)
		{
			var nativeItems = new List<NativeMenuItem>();
			for(int i = 0; i < TeleporterPositions.Count; i++)
			{
				nativeItems.Add(new($"Etage #{i + 1}", true, "Server:Police:Teleport", i));
			}

			player.ShowNativeMenu(true, new("LSPD Fahrstuhl", nativeItems));
		}

		private static void TakeLicense(RPPlayer player, int targetId, int type)
		{
			if ((player.TeamId < 1 || player.TeamId > 2) && player.TeamId != 5) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || account.TeamRank < 4) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.DbId == targetId);
			if (target == null)
			{
				player.Notify("Information", "Der Spieler konnte nicht gefunden werden!", NotificationType.ERROR);
				return;
			}

			var license = LicenseService.Get(target.LicenseId);
			if (license == null) return;

			switch (type)
			{
				case 1:
					license.Car = false;
					license.CarRevoked = DateTime.Now;
					break;
				case 2:
					license.Truck = false;
					license.TruckRevoked = DateTime.Now;
					break;
				case 3:
					license.Heli = false;
					license.HeliRevoked = DateTime.Now;
					break;
				case 4:
					license.Plane = false;
					license.PlaneRevoked = DateTime.Now;
					break;
				case 5:
					license.Boat = false;
					license.BoatRevoked = DateTime.Now;
					break;
				case 6:
					license.Taxi = false;
					license.TaxiRevoked = DateTime.Now;
					break;
				case 7:
					license.Lawyer = false;
					license.LawyerRevoked = DateTime.Now;
					break;
				case 8:
					license.Gun = false;
					license.GunRevoked = DateTime.Now;
					break;
			}

			LicenseService.Update(license);
			player.Notify("Information", $"Du hast {target.Name} die Lizenz entzogen!", NotificationType.SUCCESS);
			target.Notify("Information", $"Dir wurde eine Lizenz entzogen!", NotificationType.WARN);
		}

		public static void SetSWATDuty(RPPlayer player, bool state)
		{
			if (player.TeamId < 0 || player.TeamId > 2 || player.SWATDuty == state) return;

			if(!SWATStatus && state)
			{
				player.Notify("Information", "Das SWAT ist aktuell nicht ausgerufen!", NotificationType.ERROR);
				return;
			}

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if (!state)
			{
				LoadoutService.ClearPlayerLoadout(player.DbId, true);
				PlayerController.ApplyPlayerLoadout(player);
			}
			
			player.SWATDuty = state;
			account.SWATDuty = state;
			BroadcastSWAT($"{player.Name} hat den SWAT Dienst {(player.SWATDuty ? "angetreten" : "verlassen")}!", NotificationType.INFO);

			AccountService.Update(account);
		}

		private static void BuyItem(RPPlayer player, int itemId, int amount)
		{
			if (!player.SWATDuty) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = InventoryService.GetItem(itemId);
			if (item == null) return;

			InventoryController.AddItem(inventory, item, amount);
			player.Notify("SWAT", $"Du hast {amount}x {item.Name} aus dem SWAT Schrank entnommen", NotificationType.SUCCESS);
		}

		private static void OpenShop(RPPlayer player)
		{
			if(player.TeamId < 1 || player.TeamId > 2) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || !account.TeamStorage) return;

			List<NativeMenuItem> items = null!;

			if (account.SWATDuty)
			{
				items = new()
				{
					new("SWAT Dienst verlassen", true, "Server:SWAT:SetSWATDuty", false),
					new("Schutzweste Eng entnehmen", false, "Server:SWAT:Buy", 315, 5),
					new("Schutzweste Breit entnehmen", false, "Server:SWAT:Buy", 316, 5),
					new("Spezialcarbine entnehmen", false, "Server:SWAT:Buy", 184, 1),
					new("Spezialcarbine Munition entnehmen", false, "Server:SWAT:Buy", 243, 20),
					new("Einsatzkleidung (Männlich) entnehmen", false, "Server:SWAT:Buy", 317, 1),
					new("Einsatzkleidung (Weiblich) entnehmen", false, "Server:SWAT:Buy", 318, 1)
				};
			}
			else
			{
				items = new()
				{
					new("SWAT Dienst betreten", true, "Server:SWAT:SetSWATDuty", true),
				};
			}

			player.ShowNativeMenu(true, new("SWAT Schrank", items));
		}

		private static void BroadcastSWAT(string message, NotificationType type)
		{
			foreach(var player in RPPlayer.All)
			{
				if (!player.SWATDuty) continue;
				player.Notify("SWAT", message, type);
			}
		}
	}
}