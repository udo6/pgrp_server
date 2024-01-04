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
		private static readonly int ArmorDrawable = 16;
		private static readonly int ArmorTexture = 2;
		private static readonly uint ArmorDlc = 0;

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
				var armor = (ushort)(attribute == null ? 150 : attribute.Value);

				player.AllowedHealth = player.Health + armor;
				player.Armor = armor;
				player.SetClothing(9, ArmorDrawable, ArmorTexture, ArmorDlc);
				player.VestItemId = ItemId;
				InventoryController.RemoveItem(inventory, item.Slot, 1);
			}, 4000);
		}
	}
}