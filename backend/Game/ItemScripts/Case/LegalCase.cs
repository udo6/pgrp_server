using Core.Entities;
using Core.Enums;
using Database.Models.Case;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts.Case
{
	public class LegalCase : ItemScript
	{
		public LegalCase() : base(372, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			var loot = GetLoot();
			if (loot == null) return;

			var lootItem = InventoryService.GetItem(loot.ItemId);
			if (lootItem == null) return;

			var inventoryItems = InventoryService.GetInventoryItems(inventory.Id);

			if (InventoryController.CalcInventoryWeight(inventory) + (lootItem.Weight * loot.ItemAmount) > inventory.MaxWeight)
			{
				player.Notify("Information", "Du hast nicht genug Platz!", NotificationType.ERROR);
				return;
			}

			if (inventoryItems.Count + Math.Ceiling((decimal)(loot.ItemAmount / lootItem.StackSize)) > inventory.Slots)
			{
				player.Notify("Information", "Du hast nicht genug Platz!", NotificationType.ERROR);
				return;
			}

			InventoryController.RemoveItem(inventory, slot, 1);
			InventoryController.AddItem(inventory, lootItem, loot.ItemAmount);
			player.Notify("Information", $"Du hast {loot.ItemAmount}x {lootItem.Name} bekommen!", Core.Enums.NotificationType.SUCCESS);
		}

		public CaseLootModel? GetLoot()
		{
			var loot = CaseService.GetFromCase(ItemId);
			float randomValue = new Random().NextSingle();

			float cumulativeProbability = 0;
			foreach (var lootItem in loot)
			{
				cumulativeProbability += lootItem.Probability;
				if (randomValue < cumulativeProbability)
					return lootItem;
			}

			return null;
		}
	}
}
