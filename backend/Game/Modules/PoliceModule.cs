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
		public static Position ShopPosition = new(858.2901f, -1321.3055f, 28.134033f);

		[Initialize]
		public static void Initialize()
		{
			var shape = (RPShape)Alt.CreateColShapeCylinder(ShopPosition.Down(), 2f, 2f);
			shape.Id = 1;
			shape.ShapeType = ColshapeType.SWAT_SHOP;
			shape.Size = 2f;

			Alt.OnClient<RPPlayer, int, int>("Server:Police:TakeLicense", TakeLicense);
			Alt.OnClient<RPPlayer>("Server:SWAT:OpenShop", OpenShop);
			Alt.OnClient<RPPlayer, bool>("Server:SWAT:SetSWATDuty", SetSWATDuty);
			Alt.OnClient<RPPlayer, int, int>("Server:SWAT:Buy", BuyItem);
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
			if (player.SWATDuty == state) return;

			if(!SWATStatus && state)
			{
				player.Notify("Information", "Das SWAT ist aktuell nicht ausgerufen!", NotificationType.ERROR);
				return;
			}

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if (!state)
			{
				player.RemoveAllWeapons(true);
				player.Weapons.Clear();
				LoadoutService.ClearPlayerLoadout(player.DbId);
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