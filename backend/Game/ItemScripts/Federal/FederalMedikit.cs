using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Game.Controllers;

namespace Game.ItemScripts.Federal
{
    public class FederalMedikit : ItemScript
	{
		public FederalMedikit() : base(173, true)
		{

		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.Interaction || player.IsInVehicle || !player.TeamDuty) return;

			player.PlayAnimation(AnimationType.USE_MEDIKIT);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;

				player.AllowedHealth = 200 + player.Armor;
				player.Health = 200;
				InventoryController.RemoveItem(inventory, item.Slot, 1);
			}, 4000);
		}
	}
}