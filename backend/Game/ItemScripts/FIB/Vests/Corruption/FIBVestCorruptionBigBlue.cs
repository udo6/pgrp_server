using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts.FIB.Vests.Default
{
    public class FIBVestCorruptionBigBlue : ItemScript
    {
        private static readonly int ArmorDrawable = 2;
        private static readonly int ArmorTexture = 15;
        private static readonly uint ArmorDlc = 3602239810;

        public FIBVestCorruptionBigBlue() : base(162, true)
        {
        }

        public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
        {
            if (player.Interaction || player.IsInVehicle || player.TeamId != 2 || !player.TeamDuty) return;

            player.PlayAnimation(AnimationType.USE_VEST);
            player.StartInteraction(() =>
            {
                if (player == null || !player.Exists) return;

                var clothes = ClothesService.Get(player.ClothesId);
                if (clothes == null) return;

                if (clothes.Armor != ArmorDrawable || clothes.ArmorColor != ArmorTexture || clothes.ArmorDlc != ArmorDlc)
                {
                    clothes.Armor = ArmorDrawable;
                    clothes.ArmorColor = ArmorTexture;
                    clothes.ArmorDlc = ArmorDlc;
                    ClothesService.Update(clothes);
                }

				var attribute = InventoryService.GetItemAttributeByItem(item.Id);

				player.SetArmor((ushort)(attribute == null ? 100 : attribute.Value));
				player.SetClothing(9, ArmorDrawable, ArmorTexture, ArmorDlc);
				player.VestItemId = ItemId;
				InventoryController.RemoveItem(inventory, item.Slot, 1);
			}, 4000);
        }
    }
}