using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class ProcessorModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in ProcessorService.GetAll())
				ProcessorController.LoadProcessor(model);

			Alt.OnClient<RPPlayer>("Server:Processor:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:Processor:ProcessInventory", ProcessInventory);
			Alt.OnClient<RPPlayer, int, int>("Server:Processor:ProcessVehicle", ProcessVehicle);
		}

		private static void Open(RPPlayer player)
		{
			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.PROCESSOR);
			if (shape == null) return;

			var vehicles = RPVehicle.All.Where(x => x.DbId > 0 && x.Dimension == player.Dimension && VehicleController.IsVehicleOwner(x, player) && x.Position.Distance(player.Position) <= 30f);
			var nativeItems = new List<NativeMenuItem>()
			{
				new("Inventar", true, "Server:Processor:ProcessInventory", shape.ShapeId)
			};

			foreach(var item in vehicles)
			{
				var baseModel = VehicleService.GetBase(item.BaseId);
				if (baseModel == null) continue;

				nativeItems.Add(new($"{baseModel.Name}({item.DbId})", true, "Server:Processor:ProcessVehicle", shape.ShapeId, item.DbId));
			}

			player.ShowNativeMenu(true, new("Verarbeiter", nativeItems));
		}

		private static void ProcessInventory(RPPlayer player, int processorId)
		{
			var model = ProcessorService.Get(processorId);
			if (model == null) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			ProcessorController.StartProcessing(player, player, model, inventory);
		}

		private static void ProcessVehicle(RPPlayer player, int processorId, int vehicleId)
		{
			var model = ProcessorService.Get(processorId);
			if (model == null) return;

			var vehicle = RPVehicle.All.FirstOrDefault(x => x.DbId == vehicleId);
			if (vehicle == null || !VehicleController.IsVehicleOwner(vehicle, player)) return;

			var inventory = InventoryService.Get(vehicle.TrunkId);
			if (inventory == null) return;

			ProcessorController.StartProcessing(player, vehicle, model, inventory);
		}
	}
}