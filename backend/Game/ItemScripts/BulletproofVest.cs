﻿using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts
{
    public class BulletproofVest : ItemScript
	{
		private static readonly int MaleArmorDrawable = 15;
		private static readonly int MaleArmorTexture = 2;
		private static readonly uint MaleArmorDlc = 0;


		private static readonly int FemaleArmorDrawable = 17;
		private static readonly int FemaleArmorTexture = 2;
		private static readonly uint FemaleArmorDlc = 0;

		public BulletproofVest() : base(9, true)
		{

		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
            if(player.Interaction || player.IsInVehicle) return;

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
				var armor = (ushort)(attribute == null ? 100 : attribute.Value);

				player.SetArmor(armor);
				player.SetClothing(9, drawable, texture, dlc);
				player.VestItemId = ItemId;
				InventoryController.RemoveItem(inventory, item.Slot, 1);
			}, 4000);
		}
	}
}