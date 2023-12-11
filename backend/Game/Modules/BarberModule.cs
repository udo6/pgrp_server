using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules
{
	public static class BarberModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in BarberService.GetAll())
				BarberController.LoadBarber(model);

			Alt.OnClient<RPPlayer, int>("Server:Barber:Open", Open);
			Alt.OnClient<RPPlayer, int, int, int>("Server:Barber:Buy", Buy);
			Alt.OnClient<RPPlayer>("Server:Barber:Reset", Reset);
		}

		private static void Open(RPPlayer player, int barberId)
		{
			var barber = BarberService.Get(barberId);
			if (barber == null) return;

			var pos = PositionService.Get(barber.PositionId);
			if (pos == null) return;

			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			var styles = BarberService.GetStyleFromBarber(barberId, custom.Gender ? 1 : 0);
			var colors = BarberService.GetAllColors();

			player.Rotation = pos.Rotation;

			player.ShowComponent("Barber", true, JsonConvert.SerializeObject(new
			{
				Style = custom.Hair,
				Color = custom.HairColor,
				HighlightColor = custom.HairHighlightColor,
				HairStyles = styles,
				Colors = colors
			}));
		}

		private static void Buy(RPPlayer player, int styleId, int colorId, int highlightColorId)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			var price = 0;

			var style = BarberService.GetStyle(styleId);
			if (style != null)
			{
				price += style.Price;
				custom.Hair = style.Value;
			}

			var color = BarberService.GetColor(colorId);
			if(color != null)
			{
				price += color.Price;
				custom.HairColor = color.Value;
			}

			var highlightColor = BarberService.GetColor(highlightColorId);
			if (highlightColor != null)
			{
				price += highlightColor.Price;
				custom.HairHighlightColor = highlightColor.Value;
			}

			if(account.Money < price)
			{
				player.Notify("Information", "Du hast nicht genug Geld dabei!", Core.Enums.NotificationType.ERROR);
				return;
			}

			PlayerController.RemoveMoney(player, price);
			CustomizationService.Update(custom);
			PlayerController.ApplyPlayerCustomization(player);
		}

		private static void Reset(RPPlayer player)
		{
			PlayerController.ApplyPlayerCustomization(player);
		}
	}
}