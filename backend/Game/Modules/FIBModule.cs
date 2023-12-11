using AltV.Net;
using Core.Attribute;
using Core.Entities;

namespace Game.Modules
{
	public static class FIBModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, int>("Server:FIB:LocatePlayer", LocatePlayer);
		}

		private static void LocatePlayer(RPPlayer player, int targetId)
		{
			if (player.TeamId != 2 || !player.TeamDuty) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.DbId == targetId);
			if (target == null) return;

			if(target.LastLocatedByFIB.AddMinutes(3) > DateTime.Now)
			{
				player.Notify("Information", "Die Person wurde bereits vor kurzem geortet!", Core.Enums.NotificationType.ERROR);
				return;
			}

			target.LastLocatedByFIB = DateTime.Now;
			player.Emit("Client:PlayerModule:SetWaypoint", target.Position.X, target.Position.Y);
			player.Notify("Information", $"Du hast {target.Name} geortet!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}