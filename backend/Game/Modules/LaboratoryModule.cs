using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;
using Core.Enums;
using Database.Models;

namespace Game.Modules
{
	public static class LaboratoryModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var model in TeamService.GetAllLaboratories())
				LaboratoryController.LoadLaboratory(model);

			Alt.OnClient<RPPlayer>("Server:Team:ToggleLaboratory", ToggleLaboratory);
		}

		private static void ToggleLaboratory(RPPlayer player)
		{
			if (player.TeamId < 1) return;

			var lab = TeamService.GetLaboratory(player.Dimension);
			if (lab == null) return;

			if (lab.TeamId == player.TeamId)
			{
				var team = TeamService.Get(player.TeamId);
				if (team == null || (team.Type != TeamType.GANG && team.Type != TeamType.MAFIA)) return;

				player.LabRunning = !player.LabRunning;
				player.Notify("Drogenlabor", $"Du hast das Drogenlabor {(player.LabRunning ? "eingeschaltet" : "ausgeschaltet")}!", NotificationType.INFO);
				return;
			}

			var now = DateTime.Now;

			var enemyTeamId = lab.TeamId;
			var enemies = RPPlayer.All.Where(x => x.TeamId == enemyTeamId).ToList();

			if (enemies.Count < 15)
			{
				player.Notify("Information", "Es sind nicht genug Mitglieder der gegenpartei online!", NotificationType.ERROR);
				return;
			}

			if (now.Hour < 18 || lab.LastAttack.AddHours(12) > now)
			{
				player.Notify("Information", "Das Labor ist aktuell nicht angreifbar!", NotificationType.ERROR);
				return;
			}

			TeamController.Broadcast(enemyTeamId, "Euer Labor wird ausgeräumt!", NotificationType.WARN);
			TeamController.Broadcast(player.TeamId, "Ihr räumt das Labor nun aus!", NotificationType.INFO);

			player.PlayAnimation(AnimationType.USE_VEST);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;

				var robInv = InventoryService.Get(lab.RobInventoryId);
				if (robInv == null) return;

				var itemBases = InventoryService.GetItems();

				var loot = new List<(int ItemId, int Amount)>();
				foreach(var enemy in enemies)
				{
					if(enemy == null || !enemy.Exists) continue;

					var inventoryItems = InventoryService.GetInventoryItems(enemy.LaboratoryInputInventoryId);
					inventoryItems.AddRange(InventoryService.GetInventoryItems(enemy.LaboratoryOutputInventoryId));
					foreach(var item in inventoryItems)
					{
						loot.Add((item.ItemId, item.Amount));
					}

					InventoryService.RemoveInventoryItems(inventoryItems);
				}

				foreach(var item in loot)
				{
					var itemBase = itemBases.FirstOrDefault(x => x.Id == item.ItemId);
					if (itemBase == null) continue;

					InventoryController.AddItem(robInv, itemBase, item.Amount);
				}

				TeamController.Broadcast(enemyTeamId, "Euer Labor wurde ausgeräumt!", NotificationType.WARN);
				TeamController.Broadcast(player.TeamId, "Ihr habt das Labor ausgeräumt!", NotificationType.SUCCESS);

				lab.LastAttack = now;
				TeamService.UpdateLaboratory(lab);
			}, 180000);
		}

		[EveryFifteenMinutes]
		public static void Tick()
		{
			var teams = TeamService.GetAll();

			var cokeInput = InventoryService.GetItem(277);
			var weedInput = InventoryService.GetItem(275);
			var utilityInput = InventoryService.GetItem(5);
			var cokeOutput = InventoryService.GetItem(3);
			var weedOutput = InventoryService.GetItem(276);
			if (cokeInput == null || weedInput == null || utilityInput == null || cokeOutput == null || weedOutput == null) return;

			foreach (var player in RPPlayer.All.ToList())
			{
				if (!player.LabRunning) continue;

				var team = teams.FirstOrDefault(x => x.Id == player.TeamId);
				if (team == null) continue;

				var inputItem = team.Type == TeamType.GANG ? weedInput : cokeInput;

				var items = InventoryService.GetItemsFromId(player.LaboratoryInputInventoryId, inputItem.Id, utilityInput.Id);
				var input = items.Where(x => x.ItemId == inputItem.Id).Sum(x => x.Amount);
				var utility = items.Where(x => x.ItemId == utilityInput.Id).Sum(x => x.Amount);

				if(input < 20 || utility < 10)
				{
					player.LabRunning = false;
					player.Notify("Drogenlabor", "Das Labor hat keine Ressourcen mehr!", NotificationType.ERROR);
					continue;
				}

				var inputInventory = InventoryService.Get(player.LaboratoryInputInventoryId);
				var outputInvnetory = InventoryService.Get(player.LaboratoryInputInventoryId);
				if (inputInventory == null || outputInvnetory == null) continue;

				var outputItem = team.Type == TeamType.GANG ? weedOutput : cokeOutput;

				InventoryController.RemoveItem(inputInventory, inputItem, 20);
				InventoryController.RemoveItem(inputInventory, utilityInput, 10);
				InventoryController.AddItem(outputInvnetory, outputItem, 1);
			}
		}
	}
}