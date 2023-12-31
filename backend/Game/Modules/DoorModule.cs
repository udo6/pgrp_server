using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Models.Door;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Modules
{
	public static class DoorModule
	{
		public static string JSONData = string.Empty;

		[Initialize]
		public static void Initialize()
		{
			var doors = DoorService.GetAll();

			foreach (var model in doors)
				DoorController.LoadDoor(model);

			JSONData = GetData(doors);

			Alt.OnClient<RPPlayer>("Server:Door:Lock", Lock);
		}

		private static void Lock(RPPlayer player)
		{
			if (!player.LoggedIn) return;

			var shape = RPShape.Get(player.Position, player.Dimension, Core.Enums.ColshapeType.DOOR);
			if (shape == null) return;

			var door = DoorService.Get(shape.ShapeId);
			if (door == null || !DoorController.HasDoorAccess(door.Id, player.DbId, player.TeamId)) return;

			var state = !door.Locked;

			player.Notify("Information", $"Tür {(state ? "abgeschlossen" : "aufgeschlossen")}.", state ? Core.Enums.NotificationType.ERROR : Core.Enums.NotificationType.SUCCESS);
			Alt.EmitAllClients("Client:DoorModule:Lock", door.Id, state);

			door.Locked = state;
			DoorService.Update(door);
		}

		private static string GetData(List<DoorModel> doors)
		{
			var data = new List<object>();
			foreach(var door in doors)
			{
				data.Add(new
				{
					Id = door.Id,
					Locked = door.Locked,
					Doors = GetEntityData(door.Id)
				});
			}

			return JsonConvert.SerializeObject(data);
		}

		private static List<object> GetEntityData(int doorId)
		{
			var entites = DoorService.GetEntities(doorId);
			var data = new List<object>();
			foreach(var entity in entites)
			{
				var pos = PositionService.Get(entity.PositionId);
				if (pos == null) continue;

				data.Add(new
				{
					Model = entity.Model,
					Position = pos.Position
				});
			}

			return data;
		}
	}
}
