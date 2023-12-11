using Core;
using Core.Entities;
using Database.Models.Inventory;
using Game.Modules.Scenario;

namespace Game.ItemScripts
{
    public class HackingKit : ItemScript
	{
		public HackingKit() : base(278, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.TeamId <= 5) return;

			if(player.Position.Distance(FederalBankScenarioModule.HackPosition) <= 3f)
			{
				if (FederalBankScenarioModule.HasBeenAttacked)
				{
					player.Notify("Information", "Die Staatsbank wurde vor kurzem erst ausgeraubt!", Core.Enums.NotificationType.ERROR);
					return;
				}

				var cops = RPPlayer.All.Count(x => x.TeamId > 0 && x.TeamId < 3);
				if (cops < 10 && !Config.DevMode)
				{
					player.Notify("Information", "Die Staatsbank ist aktuell leer! (Weniger als 10 Polizisten im Dienst)", Core.Enums.NotificationType.ERROR);
					return;
				}

				FederalBankScenarioModule.StartHacking(player);
			}

			if (player.Position.Distance(JeweleryScenarioModule.HackPosition) <= 3f)
			{
				if (JeweleryScenarioModule.HasBeenAttacked)
				{
					player.Notify("Information", "Der Juwelier wurde vor kurzem erst ausgeraubt!", Core.Enums.NotificationType.ERROR);
					return;
				}

				var cops = RPPlayer.All.Count(x => x.TeamId > 0 && x.TeamId < 3);
				if (cops < 10 && !Config.DevMode)
				{
					player.Notify("Information", "Die Staatsbank ist aktuell leer! (Weniger als 10 Polizisten im Dienst)", Core.Enums.NotificationType.ERROR);
					return;
				}

				JeweleryScenarioModule.StartHacking(player);
			}
		}
	}
}