using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Newtonsoft.Json;

namespace Game.Modules
{
	public static class MMenuModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:MMenu:Open", Open);
			Alt.OnClient<RPPlayer>("Server:MMenu:ViewIdCard", ViewIdCard);
			Alt.OnClient<RPPlayer>("Server:MMenu:ViewLicenses", ViewLicense);
			Alt.OnClient<RPPlayer, int>("Server:MMenu:ToggleClothes", ToggleClothes);
		}

		private static void Open(RPPlayer player)
		{
			player.ShowComponent("MMenu", true, JsonConvert.SerializeObject(new
			{
				Clothes = new
				{
					Hat = player.HatState,
					Mask = player.MaskState,
					Glasses = player.GlassesState,
					Ears = player.EarsState,
					Top = player.TopState,
					Undershirt = player.UndershirtState,
					Watch = player.WatchState,
					Bracelet = player.BraceletState,
					Pants = player.PantsState,
					Shoes = player.ShoesState,
					Accessories = player.AccessoriesState,
					Decals = player.DecalsState
				}
			}));
		}

		private static void ViewIdCard(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			player.EmitBrowser("Hud:ShowIdCard", true, JsonConvert.SerializeObject(new
			{
				Name = account.Name,
				Date = account.Created,
				Level = account.Level,
				Gender = custom.Gender
			}));
		}

		private static void ViewLicense(RPPlayer player)
		{
			var licenses = LicenseService.Get(player.LicenseId);
			if (licenses == null) return;

			player.EmitBrowser("Hud:ShowLicense", true, JsonConvert.SerializeObject(new
			{
				Name = player.Name,
				Car = licenses.Car,
				Truck = licenses.Truck,
				Heli = licenses.Heli,
				Plane = licenses.Plane,
				Boat = licenses.Boat,
				Taxi = licenses.Taxi,
				Lawyer = licenses.Lawyer,
				Gun = licenses.Gun
			}));
		}

		private static void ToggleClothes(RPPlayer player, int id)
		{
			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			var clothes = ClothesService.Get(player.ClothesId);
			if (clothes == null) return;

			switch (id)
			{
				case 1:
					player.HatState = !player.HatState;
					player.SetProp(0, player.HatState ? clothes.Hat : -1, player.HatState ? clothes.HatColor : 0, player.HatState ? clothes.HatDlc : 0);
					break;
				case 2:
					player.MaskState = !player.MaskState;
					player.SetClothing(1, player.MaskState ? clothes.Mask : 0, player.MaskState ? clothes.MaskColor : 0, player.MaskState ? clothes.MaskDlc : 0);
					break;
				case 3:
					player.GlassesState = !player.GlassesState;
					player.SetProp(1, player.GlassesState ? clothes.Glasses : -1, player.GlassesState ? clothes.GlassesColor : 0, player.GlassesState ? clothes.GlassesDlc : 0);
					break;
				case 4:
					player.EarsState = !player.EarsState;
					player.SetProp(2, player.EarsState ? clothes.Mask : -1, player.EarsState ? clothes.EarsColor : 0, player.EarsState ? clothes.EarsDlc : 0);
					break;
				case 5:
					player.TopState = !player.TopState;
					player.SetClothing(11, player.TopState ? clothes.Top : 15, player.TopState ? clothes.TopColor : 0, player.TopState ? clothes.TopDlc : 0);
					player.SetClothing(3, player.TopState ? clothes.Body : 15, player.TopState ? clothes.BodyColor : 0, player.TopState ? clothes.BodyDlc : 0);
					break;
				case 6:
					player.UndershirtState = !player.UndershirtState;
					player.SetClothing(8, player.UndershirtState ? clothes.Undershirt : 15, player.UndershirtState ? clothes.UndershirtColor : 0, player.UndershirtState ? clothes.UndershirtDlc : 0);
					break;
				case 7:
					player.WatchState = !player.WatchState;
					player.SetProp(6, player.WatchState ? clothes.Watch : -1, player.WatchState ? clothes.WatchColor : 0, player.WatchState ? clothes.WatchDlc : 0);
					break;
				case 8:
					player.BraceletState = !player.BraceletState;
					player.SetProp(7, player.BraceletState ? clothes.Bracelet : -1, player.BraceletState ? clothes.BraceletColor : 0, player.BraceletState ? clothes.BraceletDlc : 0);
					break;
				case 9:
					player.PantsState = !player.PantsState;
					player.SetClothing(4, player.PantsState ? clothes.Pants : 21, player.PantsState ? clothes.PantsColor : 0, player.PantsState ? clothes.PantsDlc : 0);
					break;
				case 10:
					player.ShoesState = !player.ShoesState;
					player.SetClothing(6, player.ShoesState ? clothes.Shoes : custom.Gender ? 34 : 35, player.ShoesState ? clothes.ShoesColor : 0, player.ShoesState ? clothes.ShoesDlc : 0);
					break;
				case 11:
					player.AccessoriesState = !player.AccessoriesState;
					player.SetClothing(7, player.AccessoriesState ? clothes.Accessories : 0, player.AccessoriesState ? clothes.AccessoriesColor : 0, player.AccessoriesState ? clothes.AccessoriesDlc : 0);
					break;
				case 12:
					player.DecalsState = !player.DecalsState;
					player.SetClothing(10, player.DecalsState ? clothes.Decals : 0, player.DecalsState ? clothes.DecalsColor : 0, player.DecalsState ? clothes.DecalsDlc : 0);
					break;
			}
		}
	}
}
