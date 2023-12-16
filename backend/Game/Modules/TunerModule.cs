using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Models.NativeMenu;
using Database.Models.Tuner;
using Database.Services;
using Game.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Modules
{
	public static class TunerModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach (var model in TunerService.GetAll())
				TunerController.LoadTuner(model);

			Alt.OnClient<RPPlayer>("Server:Tuner:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:Tuner:SelectCategory", SelectCategory);
			Alt.OnClient<RPPlayer, int>("Server:Tuner:BuyItem", BuyItem);
		}

		private static void Open(RPPlayer player)
		{
			if (player.TeamId != 4 || !player.IsInVehicle) return;

			var categories = TunerService.GetAllCategories();
			var nativeItems = new List<NativeMenuItem>();
			foreach(var category in categories)
			{
				nativeItems.Add(new(category.Name, false, "Server:Tuner:SelectCategory", category.Id));
			}

			player.ShowNativeMenu(true, new("Los Santos Customs", nativeItems));
		}

		private static void SelectCategory(RPPlayer player, int categoryId)
		{
			if (player.TeamId != 4 || !player.IsInVehicle) return;

			var category = TunerService.GetCategory(categoryId);
			if (category == null) return;

			var items = TunerService.GetItems(categoryId);
			var nativeItems = new List<NativeMenuItem>();
			foreach (var item in items)
			{
				nativeItems.Add(new(item.Name, false, "Server:Tuner:BuyItem", item.Id));
			}

			player.ShowNativeMenu(true, new(category.Name, nativeItems));
		}

		private static void BuyItem(RPPlayer player, int itemId)
		{
			if (player.TeamId != 4 || !player.IsInVehicle) return;

			var item = TunerService.GetItem(itemId);
			if (item == null) return;

			ApplyTuning(player, (RPVehicle)player.Vehicle, item);
			player.Notify("Information", $"Du hast {item.Name} eingebaut!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void ApplyTuning(RPPlayer player, RPVehicle vehicle, TunerItemModel item)
		{
			var tuning = TuningService.Get(vehicle.TuningId);
			if (tuning == null) return;

			var val = (byte)item.ModValue;

			switch (item.ModCategory)
			{
				case 0:
					vehicle.SetMod(0, val);
					tuning.Spoiler = val;
					break;
				case 1:
					vehicle.SetMod(1, val);
					tuning.FrontBumper = val;
					break;
				case 2:
					vehicle.SetMod(2, val);
					tuning.RearBumper = val;
					break;
				case 3:
					vehicle.SetMod(3, val);
					tuning.SideSkirt = val;
					break;
				case 4:
					vehicle.SetMod(4, val);
					tuning.Exhaust = val;
					break;
				case 5:
					vehicle.SetMod(5, val);
					tuning.Frame = val;
					break;
				case 6:
					vehicle.SetMod(6, val);
					tuning.Grille = val;
					break;
				case 7:
					vehicle.SetMod(7, val);
					tuning.Hood = val;
					break;
				case 8:
					vehicle.SetMod(8, val);
					tuning.Fender = val;
					break;
				case 9:
					vehicle.SetMod(9, val);
					tuning.RightFender = val;
					break;
				case 10:
					vehicle.SetMod(10, val);
					tuning.Roof = val;
					break;
				case 11:
					vehicle.SetMod(11, val);
					tuning.Engine = val;
					break;
				case 12:
					vehicle.SetMod(12, val);
					tuning.Brakes = val;
					break;
				case 13:
					vehicle.SetMod(13, val);
					tuning.Transmission = val;
					break;
				case 14:
					vehicle.SetMod(14, val);
					tuning.Horns = val;
					break;
				case 15:
					vehicle.SetMod(15, val);
					tuning.Suspension = val;
					break;
				case 16:
					vehicle.SetMod(16, val);
					tuning.Armor = val;
					break;
				case 18:
					vehicle.SetMod(18, val);
					tuning.Turbo = val;
					break;
				case 22:
					vehicle.SetMod(22, val);
					tuning.Xenon = val;
					break;
				case 23:
					vehicle.SetWheels(tuning.WheelType, val);
					tuning.Wheels = val;
					break;
				case 25:
					vehicle.SetMod(25, val);
					tuning.PlateHolders = val;
					break;
				case 27:
					vehicle.SetMod(27, val);
					tuning.TrimDesign = val;
					break;
				case 48:
					vehicle.SetMod(48, val);
					tuning.Livery = val;
					break;
				case 53:
					vehicle.NumberplateIndex = val;
					tuning.PlateHolders = val;
					break;
				case 1000:
					vehicle.PrimaryColor = val;
					tuning.PrimaryColor = val;
					break;
				case 1001:
					vehicle.SecondaryColor = val;
					tuning.SecondaryColor = val;
					break;
				case 1002:
					vehicle.PearlColor = val;
					tuning.PearlColor = val;
					break;
				case 1004:
					vehicle.WindowTint = val;
					tuning.WindowTint = val;
					break;
				case 1005:
					vehicle.HeadlightColor = val;
					tuning.HeadlightColor = val;
					break;
				case 1006:
					vehicle.WheelColor = val;
					tuning.WheelColor = val;
					break;
				case 1007:
					vehicle.SetWheels(val, tuning.Wheels);
					tuning.WheelType = val;
					break;
			}

			TuningService.Update(tuning);
		}
	}
}
