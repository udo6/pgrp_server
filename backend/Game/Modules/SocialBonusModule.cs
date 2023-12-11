using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Models;
using Core.Enums;
using Core.Extensions;
using Database.Services;
using Newtonsoft.Json;
using Game.Controllers;

namespace Game.Modules
{
	public static class SocialBonusModule
	{
		private static PositionModel Position = new(246.47473f, 214.00879f, 106.28357f, 0f, 0f, -0.2968434f);

		[Initialize]
		public static void Initialize()
		{
			var shape = (RPShape)Alt.CreateColShapeCylinder(Position.Position.Down(), 2f, 2f);
			shape.Id = 1;
			shape.ShapeType = ColshapeType.SOCIAL_BONUS;
			shape.Size = 2f;

			var ped = Alt.CreatePed(3446096293, Position.Position, Position.Rotation);
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;

			Alt.OnClient<RPPlayer>("Server:SocialBonus:Open", Open);
			Alt.OnClient<RPPlayer>("Server:SocialBonus:Take", Take);
		}

		private static void Open(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Sozialbonus",
				Message = $"Möchtest du deinen Sozialbonus in höhe von ${account.SocialBonusMoney} abheben?",
				Type = (int)InputType.CONFIRM,
				CallbackEvent = "Server:SocialBonus:Take",
				CallbackArgs = new List<object>()
			}));
		}

		private static void Take(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			PlayerController.AddMoney(player, account.SocialBonusMoney);

			account.SocialBonusMoney = 0;
			AccountService.Update(account);
			player.Notify("Information", $"Du hast deinen Sozialbonus abgeholt!", NotificationType.SUCCESS);
		}
	}
}
