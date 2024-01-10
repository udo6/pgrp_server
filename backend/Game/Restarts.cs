using Core.Attribute;
using Core.Entities;
using Discord;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game
{
	public static class Restarts
	{
		private static readonly DateTime ProgramStarted = DateTime.Now;

		[EveryMinute]
		public static void Tick()
		{
			var now = DateTime.Now;
			if (ProgramStarted.AddMinutes(3) > now) return;

			if(now.Hour == 13 || now.Hour == 1)
			{
				switch(now.Minute)
				{
					case 50:
						AnnounceRestart(10);
						break;
					case 55:
						AnnounceRestart(5);
						break;
					case 57:
						AnnounceRestart(3);
						break;
					default:
						break;
				}
			}

			if((now.Hour != 14 && now.Hour != 2) || now.Minute > 1) return;

			Restart();
		}

		public static void Restart()
		{
			KickAllPlayers();
			Updater.UpdateVehicles();
			Updater.UpdateLogs();

			Environment.Exit(0);
		}

		public static void AnnounceRestart(int time)
		{
			var data = JsonConvert.SerializeObject(new
			{
				Title = "Globale Nachricht",
				Message = $"Ein geplanter Server-Restart findet in {time} Minuten statt!",
				Duration = 10000
			});

			foreach (var target in RPPlayer.All.ToList())
			{
				target.EmitBrowser("Hud:ShowGlobalNotification", data);
			}
		}

		private static void KickAllPlayers()
		{
			foreach(var player in RPPlayer.All.ToList())
			{
				player.Kick("Der Server wird neugestartet!");
			}
		}
	}
}
