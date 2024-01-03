using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Core.Models.NativeMenu;
using Database.Models;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class DMVModule
	{
		private static PositionModel Position = new(-815.26154f, -1346.7825f, 5.134033f, 0f, 0f, 0.7915824f);

		[Initialize]
		public static void Initialize()
		{
			var shape = (RPShape)Alt.CreateColShapeCylinder(Position.Position.Down(), 2f, 2f);
			shape.ShapeId = 1;
			shape.ShapeType = ColshapeType.DMV;
			shape.Size = 2f;

			var ped = Alt.CreatePed(3446096293, Position.Position, Position.Rotation);
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;

			Alt.OnClient<RPPlayer>("Server:DMV:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:DMV:BuyLicense", BuyLicense);
		}

		private static void Open(RPPlayer player)
		{
			var license = LicenseService.Get(player.LicenseId);
			if (license == null) return;

			var nativeItems = new List<NativeMenuItem>();
			var now = DateTime.Now;

			if (!license.Car && license.CarRevoked.AddDays(3) < now)
			{
				nativeItems.Add(new($"PKW Führerschein - ${GetLicensePrice(0)}", true, "Server:DMV:BuyLicense", 0));
			}

			if (!license.Truck && license.TruckRevoked.AddDays(3) < now)
			{
				nativeItems.Add(new($"LKW Führerschein - ${GetLicensePrice(1)}", true, "Server:DMV:BuyLicense", 1));
			}

			if (!license.Heli && license.HeliRevoked.AddDays(3) < now)
			{
				nativeItems.Add(new($"Helikopter Führerschein - ${GetLicensePrice(2)}", true, "Server:DMV:BuyLicense", 2));
			}

			if (!license.Plane && license.PlaneRevoked.AddDays(3) < now)
			{
				nativeItems.Add(new($"Flugzeug Führerschein - ${GetLicensePrice(3)}", true, "Server:DMV:BuyLicense", 3));
			}

			if (!license.Boat && license.BoatRevoked.AddDays(3) < now)
			{
				nativeItems.Add(new($"Boot Führerschein - ${GetLicensePrice(4)}", true, "Server:DMV:BuyLicense", 4));
			}

			player.ShowNativeMenu(true, new("Fahrschule", nativeItems));
		}

		private static void BuyLicense(RPPlayer player, int type)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.DMV);
			if (shape == null) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var price = GetLicensePrice(type);

			if (account.Money < price)
			{
				player.Notify("Information", $"Du benötigst ${price}!", NotificationType.ERROR);
				return;
			}

			var license = LicenseService.Get(player.LicenseId);
			if (license == null) return;

			PlayerController.RemoveMoney(player, price);

			switch (type)
			{
				case 0:
					license.Car = true;
					break;
				case 1:
					license.Truck = true;
					break;
				case 2:
					license.Heli = true;
					break;
				case 3:
					license.Plane = true;
					break;
				case 4:
					license.Boat = true;
					break;
			}

			LicenseService.Update(license);
		}

		private static int GetLicensePrice(int type)
		{
			switch (type)
			{
				default:
				case 0:
				case 1:
					return 1000;
				case 2:
				case 3:
					return 5000;
				case 4:
					return 3500;
			}
		}
	}
}
