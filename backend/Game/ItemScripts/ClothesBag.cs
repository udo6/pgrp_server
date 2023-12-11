using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Game.Controllers;

namespace Game.ItemScripts
{
	internal class ClothesBag : ItemScript
	{
		public ClothesBag() : base(300, true, ItemType.DEFAULT)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.TempClothesId < 1 || player.Interaction) return;

			player.PlayAnimation(AnimationType.USE_VEST);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;

				player.TempClothesId = 0;
				PlayerController.ApplyPlayerClothes(player);
				InventoryController.RemoveItem(inventory, slot, 1);
			}, 5000);
		}
	}
}
