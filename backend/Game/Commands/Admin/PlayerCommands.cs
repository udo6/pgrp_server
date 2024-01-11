using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Admin
{
	public static class PlayerCommands
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, int>("Server:PlayerCommands:Goto", GotoPlayer);
			Alt.OnClient<RPPlayer, int>("Server:PlayerCommands:Bring", BringPlayer);
			Alt.OnClient<RPPlayer, int>("Server:PlayerCommands:Revive", RevivePlayer);
			Alt.OnClient<RPPlayer, int>("Server:PlayerCommands:Respawn", RespawnPlayer);
		}

		[Command("revive")]
		public static void RevivePlayer(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Name.ToLower() == targetName.ToLower());
			if (target == null)
			{
				player.Notify("Administration", $"{targetName} konnte nicht gefunden werden!", NotificationType.ERROR);
				return;
			}

			if(target.Health < 1)
			{
				player.Notify("Administration", $"Du musst warten bis die Person respawned ist!", NotificationType.ERROR);
				return;
			}

			PlayerController.SetPlayerAlive(target, false);
		}

		[Command("reviveradius")]
		public static void ReviveAllPlayers(RPPlayer player, int radius)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			foreach (var target in RPPlayer.All.ToList())
			{
				if (target.Alive || player.Position.Distance(target.Position) > radius) continue;

				PlayerController.SetPlayerAlive(target, false);
			}

			AdminController.BroadcastTeam("Administration", $"{player.Name} hat alle Spieler wiederbelebt!", NotificationType.WARN);
		}

		[Command("reviveall")]
		public static void ReviveAllPlayers(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.SUPERADMIN) return;

			foreach(var target in RPPlayer.All.ToList())
			{
				if (target.Alive) continue;

				PlayerController.SetPlayerAlive(target, false);
			}

			AdminController.BroadcastTeam("Administration", $"{player.Name} hat alle Spieler wiederbelebt!", NotificationType.WARN);
		}

		[Command("respawn")]
		public static void RespawnPlayer(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && x.Name.ToLower() == targetName.ToLower());
			if (target == null)
			{
				player.Notify("Administration", $"{targetName} konnte nicht gefunden werden!", NotificationType.ERROR);
				return;
			}

			target.Spawn(target.Position, 0);
			target.Invincible = false;
		}

		[Command("setfood")]
		public static void SetPlayerFood(RPPlayer player, string targetName, int hunger, int thirst)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			target.Hunger = hunger;
			target.Thirst = thirst;
			target.SetFood(hunger, thirst, 110);
		}

		[Command("goto")]
		public static void GotoPlayer(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			player.AdminGoBackPosition = player.Position;
			player.SetPosition(target.Position);
			player.Dimension = target.Dimension;
		}

		[Command("bring")]
		public static void BringPlayer(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			target.SetPosition(player.Position);
		}

		[Command("spectate")]
		public static void SpectatePlayer(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			player.InInterior = true;
			player.OutsideInteriorPosition = player.Position;
			player.Emit("Client:AdminModule:StartSpectating", target);
			player.SetDimension(target.Dimension);
		}

		[Command("stopspectate")]
		public static void StopSpectatePlayer(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			player.Emit("Client:AdminModule:StopSpectating");
			player.InInterior = false;
			player.SetPosition(player.OutsideInteriorPosition);
			player.SetDimension(0);
		}

		[Command("players")]
		public static void GetAllPlayers(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var all = RPPlayer.All.Where(x => x.LoggedIn).ToList();
			var nativeItems = new List<NativeMenuItem>();
			foreach (var target in all)
			{
				nativeItems.Add(new($"{target.Name} ({target.DbId})", true, ""));
			}

			player.ShowNativeMenu(true, new($"Online Spieler (Gesamt: {all.Count})", nativeItems));
		}

		[Command("players2")]
		public static void GetAllPlayers2(RPPlayer player, string search)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var all = RPPlayer.All.Where(x => x.LoggedIn && x.Name.ToLower().Contains(search.ToLower())).ToList();
			var nativeItems = new List<NativeMenuItem>();
			foreach (var target in all)
			{
				nativeItems.Add(new($"{target.Name} ({target.DbId})", true, ""));
			}

			player.ShowNativeMenu(true, new($"Suche '{search}' ({all.Count})", nativeItems));
		}

		[Command("afind")]
		public static void FindPlayer(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.ADMINISTRATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			player.Emit("Client:PlayerModule:SetWaypoint", target.Position.X, target.Position.Y);
			player.Notify("Administration", "Du hast einen Spieler lokalisiert!", NotificationType.SUCCESS);
		}

		[Command("damagecap")]
		public static void DamageCap(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.SUPERADMIN) return;

			var account = AccountService.Get(targetName);
			if(account == null)
			{
				player.Notify("Administration", "Der Spieler konnte nicht gefunden werden!", NotificationType.ERROR);
				return;
			}

			account.DamageCap = !account.DamageCap;
			AccountService.Update(account);

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target != null)
			{
				target.Emit("Client:PlayerModule:SetSuperSecretFeature", account.DamageCap);
			}

			player.Notify("Administration", $"Das Super Secret Anticheat ist nun für {account.Name} {(account.DamageCap ? "aktiviert" : "deaktiviert")}!", NotificationType.SUCCESS);
		}

		[Command("enableweapons")]
		public static void EnableWeapons(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null) return;

			target.LastRevived = DateTime.Now.AddHours(-1);

			player.Notify("Administration", $"Du hast die Waffen für {target.Name} aktiviert!", NotificationType.SUCCESS);
		}










		// stuff

		private static void GotoPlayer(RPPlayer player, int targetId)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			player.AdminGoBackPosition = player.Position;
			player.SetPosition(target.Position);
			player.Dimension = target.Dimension;
			player.Notify("Administration", $"Du hast dich zu {target.Name} teleportiert!", NotificationType.SUCCESS);
		}

		private static void BringPlayer(RPPlayer player, int targetId)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			if (target.IsGangwar)
			{
				player.Notify("Information", "Die Person befindet sich im Gangwar!", NotificationType.ERROR);
				return;
			}

			if (target.IsInFFA)
			{
				player.Notify("Information", "Die Person befindet sich im FFA!", NotificationType.ERROR);
				return;
			}

			target.SetPosition(player.Position);
			target.Dimension = player.Dimension;
			player.Notify("Administration", $"Du hast {target.Name} zu dir teleportiert!", NotificationType.SUCCESS);
		}

		private static void RevivePlayer(RPPlayer player, int targetId)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			PlayerController.SetPlayerAlive(target, false);
			player.Notify("Administration", $"Du hast {target.Name} wiederbelebt!", NotificationType.SUCCESS);
		}

		private static void RespawnPlayer(RPPlayer player, int targetId)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			target.Spawn(target.Position, 0);
			player.Notify("Administration", $"Du hast {target.Name} respawnt!", NotificationType.SUCCESS);
		}

		// 

		[Command("kickall", true)]
		public static void KickAllPlayers(RPPlayer player, string reason)
		{
			if (player.AdminRank < AdminRank.MANAGER) return;

			foreach (var target in RPPlayer.All.ToList())
			{
				target.Kick(reason);
			}
		}

		[Command("kick")]
		public static void KickPlayer(RPPlayer player, string targetName)
		{
			if (player.AdminRank < AdminRank.SUPPORTER) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null)
			{
				player.Notify("Administration", "Es konnte kein Spieler mit dem namen gefunden werden!", NotificationType.ERROR);
				return;
			}

			target.Kick($"Du wurdest von {player.Name} vom Gameserver gekicked!");
		}

		[Command("dimension")]
		public static void KickPlayer(RPPlayer player)
		{
			if (player.AdminRank < AdminRank.GUIDE) return;

			player.Notify("Information", $"Du befindest dich in Dimension {player.Dimension}", NotificationType.INFO);
		}

		[Command("setdimension")]
		public static void KickPlayer(RPPlayer player, string targetName, int dimension)
		{
			if (player.AdminRank < AdminRank.MODERATOR) return;

            var target = RPPlayer.All.FirstOrDefault(x => x.Name.ToLower() == targetName.ToLower());
			if (target == null)
			{
				player.Notify("Administration", "Es konnte kein Spieler mit dem namen gefunden werden!", NotificationType.ERROR);
				return;
			}

			target.SetDimension(dimension);
		}
	}
}