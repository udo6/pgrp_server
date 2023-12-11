using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.ItemScripts.Federal.Vests.Chief
{
	public class LSPDVestChiefSmall : ItemScript
	{
		private static readonly int ArmorDrawable = 1;
		private static readonly int ArmorTexture = 2;
		private static readonly uint ArmorDlc = 3602239810;

		public LSPDVestChiefSmall() : base(319, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.Interaction || player.IsInVehicle || player.TeamId != 1 || !player.TeamDuty) return;

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
