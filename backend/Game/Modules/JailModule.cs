using AltV.Net;
using AltV.Net.Data;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.Account;
using Database.Services;
using Game.Controllers;
using System.Diagnostics;
using System.Security.Principal;

namespace Game.Modules
{
    public static class JailModule
	{
		private static readonly Position ReleasePosition = new(1846.3649f, 2585.9473f, 45.657837f);
		private static readonly Position ImprisonPosition = new(1690.7605f, 2591.5122f, 45.910645f);
		private static readonly Position SpawnPosition = new(1691.4462f, 2565.4812f, 45.556763f);

		[Initialize]
		public static void Initialize()
		{
			var shape = (RPShape)Alt.CreateColShapeCylinder(ImprisonPosition.Down(), 2f, 2f);
			shape.ShapeId = 1;
			shape.ShapeType = ColshapeType.JAIL;
			shape.Size = 2f;

			Alt.OnClient<RPPlayer, int>("Server:Jail:ReleaseInmate", ReleaseInmate);
			Alt.OnClient<RPPlayer, int>("Server:Jail:Imprison", ImprisonPlayer);
			Alt.OnClient<RPPlayer, int>("Server:Jail:AccessInmate", AccessInmate);
		}

		private static void ReleaseInmate(RPPlayer player, int targetId)
		{
			if ((player.TeamId < 1 || player.TeamId > 2) && player.TeamId != 5) return;

			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if(account.TeamRank < 7)
			{
				player.Notify("Information", "Du musst mind. Rang 7 sein!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			ReleasePlayer(target);
		}

		private static void AccessInmate(RPPlayer player, int targetId)
		{
			if ((player.TeamId < 1 || player.TeamId > 2) && player.TeamId != 5) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null) return;

			player.ShowNativeMenu(true, new(target.Name, new()
			{
				new("Entlassen", true, "Server:Jail:ReleaseInmate", targetId)
			}));
		}

		private static void ImprisonPlayer(RPPlayer player, int targetId)
		{
			if ((player.TeamId < 1 || player.TeamId > 2) && player.TeamId != 5) return;

			var target = RPPlayer.All.FirstOrDefault(x => x.DbId == targetId);
			if (target == null || target.Position.Distance(ImprisonPosition) > 5f)
			{
				player.Notify("Information", "Die Person muss am Inhaftierungspunkt stehen!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var crimeBases = CrimeService.GetAll();
			var crimes = CrimeService.GetPlayerCrimes(targetId);
			var jailtime = 0;
			var fine = 0;

			foreach (var crime in crimes)
			{
				var crimeBase = crimeBases.FirstOrDefault(x => x.Id == crime.CrimeId);
				if (crimeBase == null) continue;

				jailtime += crimeBase.JailTime;
				fine += crimeBase.Fine;
			}

			if(jailtime <= 0)
			{
				player.Notify("Information", "Die Person hat keine offenen Akten!", Core.Enums.NotificationType.ERROR);
				return;
			}

			var targetAccount = AccountService.Get(targetId);
			if (targetAccount == null) return;

			targetAccount.Jailtime = jailtime;
			targetAccount.BankMoney -= fine;
			AccountService.Update(targetAccount);
			BankService.AddHistory(new(targetAccount.Id, targetAccount.Name, $"Strafzettel", TransactionType.PLAYER, true, fine, DateTime.Now));
			CrimeService.RemovePlayerCrimes(targetId);
			target.Jailtime = jailtime;
			target.Notify("Information", $"Du wurdest für {jailtime} Hafteinheiten Inhaftiert!", NotificationType.INFO);

			var custom = CustomizationService.Get(target.CustomizationId);
			if (custom == null) return;

			var clothes = ClothesService.Get(target.ClothesId);
			if (clothes == null) return;

			SetJailClothes(clothes, custom.Gender);
			PlayerController.ApplyPlayerClothes(target, clothes);
			target.SetPosition(SpawnPosition);
			BroadcastImprison(player, target);

			if (targetAccount.Cuffed) PlayerController.SetPlayerCuffed(target, false);
        }

		public static void ReleasePlayer(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var crimeBases = CrimeService.GetAll();
			var crimes = CrimeService.GetPlayerCrimes(player.DbId);
			var jailtime = 0;
			var fine = 0;

			foreach (var crime in crimes)
			{
				var crimeBase = crimeBases.FirstOrDefault(x => x.Id == crime.CrimeId);
				if (crimeBase == null) continue;

				jailtime += crimeBase.JailTime;
				fine += crimeBase.Fine;
			}

			if(crimes.Count > 0)
			{
				player.Jailtime += jailtime;
				account.Jailtime = player.Jailtime;
				account.BankMoney -= fine;
				AccountService.Update(account);
				BankService.AddHistory(new(account.Id, account.Name, $"Strafzettel", TransactionType.PLAYER, true, fine, DateTime.Now));
				player.Notify("Information", $"Deine Haftzeit hat sich um {jailtime} Hafteinheiten verlängert!", NotificationType.INFO);
				return;
			}

			account.Jailtime = 0;
			AccountService.Update(account);

			player.Jailtime = 0;

			if (player.Position.Distance(SpawnPosition) < 200f)
			{
				player.SetPosition(ReleasePosition);
			}
			player.Notify("Information", "Du wurdest aus der Haft entlassen!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void SetJailClothes(ClothesModel model, bool gender)
		{
			model.Mask = 0;
			model.MaskColor = 0;
			model.MaskDlc = 0;

			model.Top = gender ? 5 : 247;
			model.TopColor = 0;
			model.TopDlc = 0;

			model.Body = gender ? 5 : 11;
			model.BodyColor = 0;
			model.BodyDlc = 0;

			model.Undershirt = 15;
			model.UndershirtColor = 0;
			model.UndershirtDlc = 0;

			model.Pants = gender ? 5 : 66;
			model.PantsColor = 7;
			model.PantsDlc = 0;

			model.Shoes = gender ? 6 : 5;
			model.ShoesColor = 0;
			model.ShoesDlc = 0;

			model.Accessories = 0;
			model.AccessoriesColor = 0;
			model.AccessoriesDlc = 0;

			model.Armor = 0;
			model.ArmorColor = 0;
			model.ArmorDlc = 0;

			model.Decals = 0;
			model.DecalsColor = 0;
			model.DecalsDlc = 0;

			model.Hat = -1;
			model.HatColor = 0;
			model.HatDlc = 0;

			model.Glasses = -1;
			model.GlassesColor = 0;
			model.GlassesDlc = 0;

			model.Ears = -1;
			model.EarsColor = 0;
			model.EarsDlc = 0;

			model.Watch = -1;
			model.WatchColor = 0;
			model.WatchDlc = 0;

			model.Bracelet = -1;
			model.BraceletColor = 0;
			model.BraceletDlc = 0;
			ClothesService.Update(model);
		}

		private static void BroadcastImprison(RPPlayer player, RPPlayer inmate)
		{
			foreach(var target in RPPlayer.All.ToList())
			{
				if ((target.TeamId < 1 || target.TeamId > 2) && target.TeamId != 5) continue;

				target.Notify("Gefängnis", $"{inmate.Name} wurde von {player.Name} inhaftiert!", NotificationType.INFO);
			}
		}

		[EveryMinute]
		public static void Tick()
		{
			foreach(var player in RPPlayer.All)
			{
				if (player.Jailtime <= 0) continue;

				player.Jailtime--;
				if(player.Jailtime <= 0)
				{
					ReleasePlayer(player);
				}
			}
		}
	}
}