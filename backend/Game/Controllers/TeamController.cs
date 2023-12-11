using AltV.Net;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Team;
using Database.Services;

namespace Game.Controllers
{
    public static class TeamController
	{
		public static void LoadTeam(TeamModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if(pos == null) return;

			var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 1.5f, 2f);
			shape.Id = model.Id;
			shape.ShapeType = ColshapeType.TEAM;
			shape.Size = 2f;
		}

		public static void SetPlayerTeam(RPPlayer player, int team, int rank, bool leader, bool storage, bool bank)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.TeamId = team;

			account.TeamId = team;
			account.TeamRank = rank;
			account.TeamAdmin = leader;
			account.TeamStorage = storage;
			account.TeamBank = bank;
			AccountService.Update(account);
			player.Emit("Client:PlayerModule:SetTeam", team);
		}

		public static int GetLaboratoryFuel(int teamId)
		{
			var lab = TeamService.GetLaboratoryByTeam(teamId);
			if(lab == null) return 0;

			var items = InventoryService.GetInventoryItems(lab.FuelInventoryId);

			return items.Count(x => x.ItemId == 2);
		}

		public static void Broadcast(int teamId, string message, NotificationType type)
		{
			foreach(var player in RPPlayer.All.ToList())
			{
				if (player.TeamId != teamId) return;
				player.Notify("Fraktion", message, type);
			}
		}

		public static void Broadcast(List<int> teamIds, string message, NotificationType type)
		{
			foreach (var player in RPPlayer.All.ToList())
			{
				if (!teamIds.Contains(player.TeamId)) continue;
				player.Notify("Fraktion", message, type);
			}
		}
	}
}