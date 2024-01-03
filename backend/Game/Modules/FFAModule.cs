using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Pools;
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
	public static class FFAModule
	{
		public static PositionModel Position = new(569.8022f, 2796.7517f, 42.001465f, 0f, 0f, 1.484217f);

		[Initialize]
		public static void Initialize()
		{
			var shape = (RPShape)Alt.CreateColShapeCylinder(Position.Position.Down(), 2f, 2f);
			shape.ShapeId = 0;
			shape.ShapeType = ColshapeType.FFA;
			shape.Size = 2f;

			var ped = Alt.CreatePed(3446096293, Position.Position.Down(), Position.Rotation);
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;

			var blip = Alt.CreateBlip(true, 4, Position.Position.Down(), Array.Empty<IPlayer>());
			blip.Name = "Paintball Arena";
			blip.Sprite = 313;
			blip.Color = 4;
			blip.ShortRange = true;

			Alt.OnClient<RPPlayer>("Server:FFA:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:FFA:Join", Join);
		}

		private static void Open(RPPlayer player)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.FFA);
			if (shape == null) return;

			var maps = FFAService.GetAll();
			var nativeItems = new List<NativeMenuItem>();
			foreach(var map in maps)
			{
				var players = RPPlayer.All.Count(x => x.LoggedIn && x.FFAId == map.Id);
				nativeItems.Add(new($"{map.Name} - {players}/{map.MaxPlayers} Spieler", true, "Server:FFA:Join", map.Id));
			}

			player.ShowNativeMenu(true, new("Paintball", nativeItems));
		}

		private static void Join(RPPlayer player, int ffaId)
		{
			var ffa = FFAService.Get(ffaId);
			if (ffa == null) return;

			if(RPPlayer.All.Count(x => x.LoggedIn && x.FFAId == ffa.Id) >= ffa.MaxPlayers)
			{
				player.Notify("Fehler", $"Es sind bereits {ffa.MaxPlayers} Spieler in der Lobby!", NotificationType.ERROR);
				return;
			}

			FFAController.JoinFFA(player, ffaId);
		}
	}
}