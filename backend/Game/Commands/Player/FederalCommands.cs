using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
using Game.Controllers;
using Game.Modules;
using Newtonsoft.Json;

namespace Game.Commands.Player
{
	public static class FederalCommands
	{
		[Command("gov", true)]
		public static void SendGovAnnounce(RPPlayer player, string message)
		{
			if(!player.LoggedIn || player.TeamId < 1 || player.TeamId > 2) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || account.TeamRank < 7) return;

			var team = TeamService.Get(player.TeamId);
			if (team == null) return;

			var data = JsonConvert.SerializeObject(new
			{
				Title = $"{team.ShortName} NACHRICHT",
				Message = message,
				Duration = 15000,
                Type = GlobalNotifyType.FEDERAL
            });

			foreach (var target in RPPlayer.All.ToList())
			{
				target.EmitBrowser("Hud:ShowGlobalNotification", data);
			}
		}

		[Command("takelic")]
		public static void TakeLicense(RPPlayer player, string name)
		{
			if (!player.LoggedIn || ((player.TeamId < 1 || player.TeamId > 2) && player.TeamId != 5)) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(account.TeamRank < 4)
			{
				player.Notify("Information", "Du musst mind. Rang 4 dafür sein!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Name.ToLower() == name.ToLower());
			if (target == null)
			{
				player.Notify("Information", "Der Spieler konnte nicht gefunden werden!", NotificationType.ERROR);
				return;
			}

			player.ShowNativeMenu(true, new(target.Name, new()
			{
				new("PKW Lizenz abnehmen", true, "Server:Police:TakeLicense", target.DbId, 1),
				new("LKW Lizenz abnehmen", true, "Server:Police:TakeLicense", target.DbId, 2),
				new("Helikopter Lizenz abnehmen", true, "Server:Police:TakeLicense", target.DbId, 3),
				new("Flugzeug Lizenz abnehmen", true, "Server:Police:TakeLicense", target.DbId, 4),
				new("Boots Lizenz abnehmen", true, "Server:Police:TakeLicense", target.DbId, 5),
				new("Taxi Lizenz abnehmen", true, "Server:Police:TakeLicense", target.DbId, 6),
				new("Anwalts Lizenz abnehmen", true, "Server:Police:TakeLicense", target.DbId, 7),
				new("Waffen Lizenz abnehmen", true, "Server:Police:TakeLicense", target.DbId, 8)
			}));
		}

		[Command("swat")]
		public static void ToggleSwatStatus(RPPlayer player)
		{
			if (!player.LoggedIn || (player.TeamId < 1 && player.TeamId > 2)) return;

			var account = AccountService.Get(player.DbId);
			if (account == null || !account.TeamAdmin) return;

			PoliceModule.SWATStatus = !PoliceModule.SWATStatus;
			TeamController.Broadcast(new List<int>() { 1, 2 }, $"{player.Name} hat das SWAT {(PoliceModule.SWATStatus ? "ausgerufen" : "abgezogen")}", NotificationType.INFO);

			if (PoliceModule.SWATStatus) return;

			foreach(var target in RPPlayer.All.ToList())
			{
				if (!target.SWATDuty) continue;
				PoliceModule.SetSWATDuty(target, false);
			}
		}
	}
}