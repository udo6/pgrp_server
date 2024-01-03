using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Models.Hospital;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class MedicModule
	{
		private static Position CreatorPosition = new(-499.68793f, -342.75165f, 34.48645f);

		[Initialize]
		public static void Initialize()
		{
			foreach(var model in HospitalService.GetAll())
				HospitalController.LoadHospital(model);

			var shape = (RPShape)Alt.CreateColShapeCylinder(CreatorPosition, 1.5f, 2f);
			shape.ShapeId = 1;
			shape.ShapeType = ColshapeType.CREATOR;
			shape.Size = 1.5f;

			var ped = Alt.CreatePed(0xE497BBEF, CreatorPosition, new(0, 0, -1.5831648f));
			ped.Frozen = true;
			ped.Health = 8000;
			ped.Armour = 8000;

			Alt.OnClient<RPPlayer>("Server:Creator:Enter", EnterCreator);
		}

		private static void EnterCreator(RPPlayer player)
		{
			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			CreatorModule.SendToCreator(player, custom, player.Position);
		}

		[Core.Attribute.ServerEvent(ServerEventType.ENTITY_COLSHAPE)]
		public static void OnColshape(RPShape shape, IWorldObject entity, bool entered)
		{
			if (!entered || shape.ShapeType != ColshapeType.HOSPITAL || entity.Type != BaseObjectType.Vehicle) return;

			var vehicle = (RPVehicle)entity;
			if (vehicle.OwnerType != OwnerType.TEAM || vehicle.OwnerId != 3) return;

			var driver = (RPPlayer)vehicle.Driver;
			if (driver == null || driver.TeamId != 3) return;

			var players = RPPlayer.All.Where(x => x.LoggedIn && x.Vehicle == vehicle && !x.Alive).ToList();
			if (players.Count < 1) return;

			var beds = HospitalService.GetBeds(shape.ShapeId);
			foreach(var player in players)
			{
				var bed = beds.FirstOrDefault(IsBedAvailable);
				if (bed == null)
				{
					driver?.Notify("Information", "Es ist kein Bett mehr frei!", NotificationType.ERROR);
					break;
				}

				HospitalController.TakeInPlayer(player, bed);
				player.Notify("Information", "Du wurdest ins Krankenhaus eingeliefert!", NotificationType.INFO);
				driver?.Notify("Information", "Du hast jemanden ins Krankenhaus eingeliefert!", NotificationType.SUCCESS);
			}
		}

		private static bool IsBedAvailable(HospitalBedModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return false;

			return !RPPlayer.All.Any(x => x.Position.Distance(pos.Position) <= 1f);
		}
	}
}