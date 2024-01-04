using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Game.Controllers;

namespace Game.ItemScripts.Gangwar
{
    public class GangwarMedikit : ItemScript
    {
        public GangwarMedikit() : base(7, true)
        {

        }

        public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
        {
            if (!player.IsGangwar || player.Interaction || player.IsInVehicle) return;

            player.PlayAnimation(AnimationType.USE_MEDIKIT);
            player.StartInteraction(() =>
            {
                if (player == null || !player.Exists || !player.IsGangwar) return;

				player.SetHealth(200);
				InventoryController.RemoveItem(inventory, item.Slot, 1);
            }, 4000);
        }
    }
}