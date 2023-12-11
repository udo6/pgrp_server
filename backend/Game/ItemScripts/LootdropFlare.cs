using Core.Entities;
using Database.Models.Inventory;
using Game.Controllers;
using Game.Modules.Scenario;

namespace Game.ItemScripts
{
	public class LootdropFlare : ItemScript
	{
		public LootdropFlare() : base(302, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			var spawned = LootdropModule.Spawn();
			if (!spawned) return;

			InventoryController.RemoveItem(inventory, slot, 1);
			player.Notify("Information", "Du hast einen Lootdrop angefordert!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
