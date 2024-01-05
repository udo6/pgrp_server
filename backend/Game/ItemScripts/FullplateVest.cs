using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;
using System;

namespace Game.ItemScripts
{
    public class FullplateVest : ItemScript
	{
		private static readonly int MaleArmorDrawable = 16;
		private static readonly int MaleArmorTexture = 2;
		private static readonly uint MaleArmorDlc = 0;


		private static readonly int FemaleArmorDrawable = 18;
		private static readonly int FemaleArmorTexture = 2;
		private static readonly uint FemaleArmorDlc = 0;

		public FullplateVest() : base(285, true)
		{

		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.Interaction || player.IsInVehicle) return;

			player.PlayAnimation(AnimationType.USE_VEST);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;

				var custom = CustomizationService.Get(player.CustomizationId);
				if (custom == null) return;

				var drawable = custom.Gender ? MaleArmorDrawable : FemaleArmorDrawable;
				var texture = custom.Gender ? MaleArmorTexture : FemaleArmorTexture;
				var dlc = custom.Gender ? MaleArmorDlc : FemaleArmorDlc;

				var clothes = ClothesService.Get(player.ClothesId);
				if (clothes == null) return;

				if (clothes.Armor != drawable || clothes.ArmorColor != texture || clothes.ArmorDlc != dlc)
				{
					clothes.Armor = drawable;
					clothes.ArmorColor = texture;
					clothes.ArmorDlc = dlc;
					ClothesService.Update(clothes);
				}

				var attribute = InventoryService.GetItemAttributeByItem(item.Id);
				var armor = (ushort)(attribute == null ? 150 : attribute.Value);

				player.SetArmor(armor);
				player.SetClothing(9, drawable, texture, dlc);
				player.VestItemId = ItemId;
				InventoryController.RemoveItem(inventory, item.Slot, 1);
			}, 4000);
		}
	}
}