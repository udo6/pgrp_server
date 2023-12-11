using AltV.Net;
using AltV.Net.Elements.Entities;
using Database.Models;
using Database.Services;

namespace Game.Controllers
{
	public static class BlipController
	{
		public static void LoadBlip(BlipModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
			blip.Name = model.Name;
			blip.Sprite = (ushort)model.Sprite;
			blip.Color = (byte)model.Color;
			blip.ShortRange = model.ShortRange;
		}
	}
}