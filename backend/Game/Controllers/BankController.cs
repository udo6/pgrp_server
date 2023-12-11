using AltV.Net;
using AltV.Net.Elements.Entities;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Bank;
using Database.Services;

namespace Game.Controllers
{
    public class BankController
	{
		public static void LoadBank(BankModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 1.5f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.BANK;
			shape.Size = 1.5f;

			if (model.Type == BankType.BANK)
			{
				var blip = Alt.CreateBlip(true, 4, pos.Position, Array.Empty<IPlayer>());
				blip.Name = $"Bank";
				blip.Sprite = 108;
				blip.Color = 2;
				blip.ShortRange = true;
			}
		}
	}
}