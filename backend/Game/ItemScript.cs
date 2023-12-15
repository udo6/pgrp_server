using Core.Entities;
using Core.Enums;
using Database.Models;
using Database.Models.Account;
using Database.Models.Inventory;
using Database.Services;
using Game.Controllers;

namespace Game
{
    public abstract class ItemScript
	{
		public int ItemId { get; }
		public bool CloseUI { get; }
		public ItemType Type { get; }

		protected ItemScript(int itemId, bool closeUI, ItemType type = ItemType.DEFAULT)
		{
			ItemId = itemId;
			CloseUI = closeUI;
			Type = type;
		}

		/// <returns>return value represent wether the item should be removed or not</returns>
		public abstract void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount);
	}

	public abstract class WeaponItemScript : ItemScript
	{
		public uint Hash { get; }
		public bool Federal { get; }
		public InjuryType Injury { get; }
		public WeaponType WeaponType { get; }

		protected WeaponItemScript(int itemId, uint hash, bool federal, InjuryType injury, WeaponType weaponType) : base(itemId, false, ItemType.WEAPON)
		{
			Hash = hash;
			Federal = federal;
			Injury = injury;
			WeaponType = weaponType;
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (Federal && !player.TeamDuty/* || player.Level < 3*/) return;

			if(player.Weapons.Any(x => x == Hash))
			{
				player.ShowComponent("Inventory", false);
				player.Notify("Information", "Du hast diese Waffe bereits ausgerüstet!", NotificationType.ERROR);
				return;
			}

			var equipped = InventoryController.GetWeaponItemScripts().Where(x => player.Weapons.Contains(x.Hash));
			if(equipped.Any(x => x.WeaponType == WeaponType && x.WeaponType != WeaponType.NONE))
			{
				player.ShowComponent("Inventory", false);
				player.Notify("Information", "Du hast bereits eine Waffe dieser Kategorie ausgerüstet!", NotificationType.ERROR);
				return;
			}

			LoadoutService.Add(new(player.DbId, Hash, 0, Federal ? LoadoutType.FEDERAL : LoadoutType.DEFAULT));
			player.AddWeapon(Hash, 0, true);
			InventoryController.RemoveItem(inventory, item.Slot, 1);
		}
	}

	public abstract class AttatchmentItemScript : ItemScript
	{
		public uint Hash { get; }
		public uint WeaponHash { get; }
		public bool Federal { get; }

		protected AttatchmentItemScript(int itemId, uint hash, uint weaponHash, bool federal = false) : base(itemId, false, ItemType.ATTATCHMENT)
		{
			Hash = hash;
			WeaponHash = weaponHash;
			Federal = federal;
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.CurrentWeapon != WeaponHash || player.Interaction || player.IsInVehicle) return;

			var loadout = LoadoutService.Get(player.CurrentWeapon);
			if (loadout == null) return;

			var attatchments = LoadoutService.GetLoadoutAttatchments(loadout.Id);
			if (attatchments.FirstOrDefault(x => x.Hash == Hash) != null) return;

			player.PlayAnimation(AnimationType.SEARCH);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;

				var comp = new LoadoutAttatchmentModel(loadout.Id, Hash);
				LoadoutService.AddAttatchment(comp);
				player.AddWeaponComponent(WeaponHash, Hash);
				InventoryController.RemoveItem(inventory, item.Slot, 1);
			}, 4000);
		}
	}

	public abstract class AmmoItemScript : ItemScript
	{
		public uint WeaponHash { get; }
		public int MagSize { get; }
		public bool Federal { get; }

		protected AmmoItemScript(int itemId, uint weaponHash, int magSize = 1, bool federal = false) : base(itemId, false, ItemType.AMMO)
		{
			WeaponHash = weaponHash;
			MagSize = magSize;
			Federal = federal;
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.CurrentWeapon != WeaponHash || Federal && !player.TeamDuty) return;

			var loadout = LoadoutService.GetLoadout(player.DbId, WeaponHash);
			if (loadout == null) return;

			var ammo = amount * MagSize;

			loadout.Ammo += ammo;
			player.AddAmmo(WeaponHash, ammo);
			LoadoutService.Update(loadout);
			InventoryController.RemoveItem(inventory, item.Slot, amount);
		}
	}

	public abstract class TempClothesItemScript : ItemScript
	{
		private int OutfitId { get; set; }

		protected TempClothesItemScript(int itemId, int outfitId) : base(itemId, true, ItemType.TEMP_CLOTHES)
		{
			OutfitId = outfitId;
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.Interaction) return;

			player.PlayAnimation(AnimationType.USE_VEST);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;

				player.TempClothesId = OutfitId;
				PlayerController.ApplyPlayerClothes(player, false);
				InventoryController.RemoveItem(inventory, slot, 1);

				var clothesReset = InventoryService.GetItem(300);
				if (clothesReset == null) return;

				InventoryController.AddItem(inventory, clothesReset, 1);
			}, 5000);
		}
	}
}