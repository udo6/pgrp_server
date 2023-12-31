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
			Alt.OnClient<RPPlayer, int, int, int, uint, int, int>("Server:Barber:Try", Try);
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

			var styles = BarberService.GetStylesFromBarber(custom.Gender ? 1 : 0);
			var colors = BarberService.GetAllColors();
			var beards = BarberService.GetBeardsFromBarber();

			player.Rotation = pos.Rotation;

			player.ShowComponent("Barber", true, JsonConvert.SerializeObject(new
			{
				Gender = custom.Gender,
				Style = custom.Hair,
				Color = custom.HairColor,
				HighlightColor = custom.HairHighlightColor,
				Beard = custom.Beard,
				BeardColor = custom.BeardColor,
				HairStyles = styles,
				Colors = colors,
				Beards = beards
			}));
		}

		private static void Try(RPPlayer player, int style, int color, int highlightColor, uint dlc, int beard, int beardColor)
		{
			player.SetClothing(2, style, 0, dlc);
			player.HairColor = (byte)color;
			player.HairHighlightColor = (byte)highlightColor;
			if(beard == -1)
			{
				player.RemoveHeadOverlay(1);
			}
			else
			{
				player.SetHeadOverlay(1, (byte)beard, 1);
				player.SetHeadOverlayColor(1, 1, (byte)beardColor, (byte)beardColor);
			}
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