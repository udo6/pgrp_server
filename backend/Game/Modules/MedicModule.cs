using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Database.Models.Hospital;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
	public static class MedicModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in HospitalService.GetAll())
				HospitalController.LoadHospital(model);
		}

		[ServerEvent(Core.Enums.ServerEventType.ENTITY_COLSHAPE)]
		public static void OnColshape(RPShape shape, IWorldObject entity, bool entered)
		{
			if (!entered || shape.ShapeType != Core.Enums.ColshapeType.HOSPITAL || entity.Type != BaseObjectType.Vehicle) return;

			var vehicle = (RPVehicle)entity;

			var players = RPPlayer.All.Where(x => x.LoggedIn && x.Vehicle == vehicle && !x.Alive).ToList();
			if (players.Count < 1) return;

			var beds = HospitalService.GetBeds(shape.Id);
			foreach(var player in players)
			{
				var driver = (RPPlayer)vehicle.Driver;
				var bed = beds.FirstOrDefault(IsBedAvailable);
				if (bed == null)
				{
					driver?.Notify("Information", "Es ist kein Bett mehr frei!", Core.Enums.NotificationType.ERROR);
					break;
				}

				HospitalController.TakeInPlayer(player, bed);
				player.Notify("Information", "Du wurdest ins Krankenhaus eingeliefert!", Core.Enums.NotificationType.INFO);
				driver?.Notify("Information", "Du hast jemanden ins Krankenhaus eingeliefert!", Core.Enums.NotificationType.SUCCESS);
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