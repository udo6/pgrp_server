using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts.Gangwar
{
    public class GangwarBulletproofVest : ItemScript
    {
        private static readonly int ArmorDrawable = 15;
        private static readonly int ArmorTexture = 2;
        private static readonly uint ArmorDlc = 0;

        public GangwarBulletproofVest() : base(6, true)
        {
        }

        public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
        {
            if (!player.IsGangwar || player.Interaction || player.IsInVehicle) return;

            player.PlayAnimation(AnimationType.USE_VEST);
            player.StartInteraction(() =>
            {
                if (player == null || !player.Exists || !player.IsGangwar) return;

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
				var armor = (ushort)(attribute == null ? 100 : attribute.Value);

				player.AllowedHealth = player.Health + armor;
				player.Armor = armor;
				player.SetClothing(9, ArmorDrawable, ArmorTexture, ArmorDlc);
				player.VestItemId = ItemId;
				InventoryController.RemoveItem(inventory, item.Slot, 1);
			}, 4000);
        }
    }
}