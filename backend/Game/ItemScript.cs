using Core.Entities;
using Core.Enums;
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

			if(player.Weapons.Any(x => x.Hash == Hash))
			{
				player.ShowComponent("Inventory", false);
				player.Notify("Information", "Du hast diese Waffe bereits ausgerüstet!", NotificationType.ERROR);
				return;
			}

			var equipped = InventoryController.GetWeaponItemScripts().Where(x => player.Weapons.Any(e => e.Hash == x.Hash));
			if(equipped.Any(x => x.WeaponType == WeaponType && x.WeaponType != WeaponType.NONE))
			{
				player.ShowComponent("Inventory", false);
				player.Notify("Information", "Du hast bereits eine Waffe dieser Kategorie ausgerüstet!", NotificationType.ERROR);
				return;
			}

			if(InventoryController.RemoveItem(inventory, item.Slot, 1))
			{
				LoadoutService.Add(new(player.DbId, Hash, 0, 0, Federal ? LoadoutType.FEDERAL : LoadoutType.DEFAULT));
				player.AddWeapon(Hash, 0, true, 0, new());
			}
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
			if (player.CurrentWeapon != WeaponHash || player.Interaction || player.IsInVehicle)
			{
				player.ShowComponent("Inventory", false);
				return;
			}

			var loadout = LoadoutService.GetLoadout(player.DbId, player.CurrentWeapon);
			if (loadout == null)
			{
				player.ShowComponent("Inventory", false);
				return;
			}

			var attatchments = LoadoutService.GetLoadoutAttatchments(loadout.Id);
			if (attatchments.FirstOrDefault(x => x.Hash == Hash) != null)
			{
				player.Notify("Information", "Du hast diesen Aufsatz bereits ausgerüstet!", NotificationType.ERROR);
				player.ShowComponent("Inventory", false);
				return;
			}

			player.PlayAnimation(AnimationType.SEARCH);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;

				if(InventoryController.RemoveItem(inventory, item.Slot, 1))
				{
					var comp = new LoadoutAttatchmentModel(loadout.Id, Hash);
					LoadoutService.AddAttatchment(comp);
					player.AddAttatchment(WeaponHash, Hash);
				}
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
			if (player.CurrentWeapon != WeaponHash || Federal && !player.TeamDuty)
			{
				player.ShowComponent("Inventory", false);
				return;
			}

			var loadout = LoadoutService.GetLoadout(player.DbId, WeaponHash);
			if (loadout == null)
			{
				player.ShowComponent("Inventory", false);
				return;
			}

			if (InventoryController.RemoveItem(inventory, item.Slot, amount))
			{
				var ammo = amount * MagSize;

				loadout.Ammo += ammo;
				player.AddAmmo(WeaponHash, ammo);
				LoadoutService.Update(loadout);
			}
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
			if (player.Interaction)
			{
				player.ShowComponent("Inventory", false);
				return;
			}

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

	public abstract class FoodItemScript : ItemScript
	{
		private int HungerGain { get; set; }
		private int ThirstGain { get; set; }

		protected FoodItemScript(int itemId, int hungerGain, int thirstGain) : base(itemId, true, ItemType.FOOD)
		{
			HungerGain = hungerGain;
			ThirstGain = thirstGain;
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.Interaction) return;

			player.Hunger = Math.Clamp(player.Hunger + HungerGain, 0, 100);
			player.Thirst = Math.Clamp(player.Thirst + ThirstGain, 0, 100);
			player.EmitBrowser("Hud:SetFood", player.Hunger, player.Thirst, 110);
			InventoryController.RemoveItem(inventory, slot, 1);
		}
	}
}