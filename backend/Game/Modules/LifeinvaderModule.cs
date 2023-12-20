using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Core.Models.Phone;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules
{
	public static class LifeinvaderModule
	{
		public static readonly List<LifeinvaderPost> Posts = new();

		private static readonly Position Position = new(-1082.1099f, -247.52966f, 37.75537f);
		private static readonly Position PedPosition = new(-1082.1099f, -247.52966f, 37.75537f);

		[Initialize]
		public static void Initialize()
		{
			var shape = (RPShape)Alt.CreateColShapeCylinder(Position.Down(), 2f, 2f);
			shape.Id = 1;
			shape.ShapeType = ColshapeType.LIFEINVADER;
			shape.Size = 2f;

			var ped = Alt.CreatePed(416176080, PedPosition, new(0, 0, -2.6221168f));
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;

			var blip = Alt.CreateBlip(true, 4, Position, Array.Empty<IPlayer>());
			blip.Name = $"Lifeinvader";
			blip.Sprite = 77;
			blip.Color = 1;
			blip.ShortRange = true;

			Alt.OnClient<RPPlayer>("Server:Lifeinvader:Open", Open);
			Alt.OnClient<RPPlayer, string>("Server:Lifeinvader:CreatePost", CreatePost);
		}

		private static void CreatePost(RPPlayer player, string text)
		{
			if(player.LastLifeinvaderPost.AddMinutes(30) > DateTime.Now)
			{
				player.Notify("Information", "Du hast in den letzten 30 Minuten bereits einen Post erstellt!", NotificationType.ERROR);
				return;
			}

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var price = text.Length * 15;
			if(account.Money < price)
			{
				player.Notify("Information", $"Du hast nicht genug Geld dabei! (${price})", NotificationType.ERROR);
				return;
			}

			player.LastLifeinvaderPost = DateTime.Now;
			PlayerController.RemoveMoney(player, price);

			var post = new LifeinvaderPost(player.DbId, text, player.PhoneNumber, DateTime.Now.ToString("HH:mm"));
			Posts.Insert(0, post);
			foreach(var user in RPPlayer.All.ToList())
			{
				if (user.AdminRank > AdminRank.SPIELER)
				{
					user.Notify("Lifeinvader", $"POST ID: {post.Id} | Spieler {player.Name}({player.DbId}): {text}", NotificationType.INFO);
					continue;
				}

				user.Notify("Lifeinvader", $"Es wurde eine neue Lifeinvader Anzeige geschaltet!", NotificationType.INFO);
			}
		}

		private static void Open(RPPlayer player)
		{
			if (player.Position.Distance(Position) > 2f) return;

			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Lifeinvader Anzeige schalten",
				Message = "Was möchtest du veröffentlichen? (Pro Zeichen $15)",
				Type = (int)InputType.TEXT,
				CallbackEvent = "Server:Lifeinvader:CreatePost",
				CallbackArgs = new List<object>()
			}));
		}
	}
}