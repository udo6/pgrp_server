using AltV.Net;
using AltV.Net.Elements.Entities;
using AltV.Net.Elements.Pools;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Inventory;
using Database.Models.Processor;
using Database.Services;

namespace Game.Controllers
{
    public static class ProcessorController
	{
		public static void LoadProcessor(ProcessorModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.PROCESSOR;
			shape.Size = 2f;

			var ped = Alt.CreatePed(3446096293, pos.Position, pos.Rotation);
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;
		}

		public static void StartProcessing(RPPlayer player, IEntity entity, ProcessorModel model, InventoryModel inventory)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null || pos.Position.Distance(entity.Position) > 30f) return;

			var items = InventoryService.GetInventoryItems(inventory.Id);

			var inputItems = items.Where(x => x.ItemId == model.InputItem);
			var inputAmount = inputItems.Sum(x => x.Amount);

			var steps = (int)Math.Floor((decimal)inputAmount / model.InputStepAmount);

			var inputItem = InventoryService.GetItem(model.InputItem);
			if (inputItem == null) return;

			var outputItem = InventoryService.GetItem(model.OutputItem);
			if (outputItem == null) return;

			if (steps < 1)
			{
				player.Notify("Verarbeiter", $"Du benötigst mind. {inputAmount} {inputItem.Name}!", NotificationType.ERROR);
				return;
			}

			player.StartInteraction(() =>
			{
				if (pos.Position.Distance(entity.Position) > 30f)
				{
					player.Notify("Information", "Du oder dein Fahrzeug hat dich zu weit vom Verarbeiter entfernt!", NotificationType.ERROR);
					return;
				}

				if(!InventoryController.RemoveItem(inventory, inputItem, model.InputStepAmount * steps))
				{
					player.Notify("Information", "Es ist ein Fehler aufgetreten!", NotificationType.ERROR);
					return;
				}

				InventoryController.AddItem(inventory, outputItem, model.OutputStepAmount * steps);
				player.Notify("Verarbeiter", $"Du hast {model.OutputStepAmount * steps} {outputItem.Name} hergestellt!", NotificationType.SUCCESS);
			}, model.Time * steps);
		}
	}
}