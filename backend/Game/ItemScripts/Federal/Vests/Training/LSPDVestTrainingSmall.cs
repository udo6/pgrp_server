using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game.ItemScripts.Federal.Vests.Training
{
	public class LSPDVestTrainingSmall : ItemScript
	{
		private static readonly int ArmorDrawable = 1;
		private static readonly int ArmorTexture = 6;
		private static readonly uint ArmorDlc = 3602239810;

		public LSPDVestTrainingSmall() : base(313, true)
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
