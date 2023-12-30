using Core.Entities;
using Core.Enums;
using Core.Models.Hud;
using Database.Models.Account;
using Database.Models.ClothesShop;
using Database.Services;
using Game.Modules;
using Logs;
using Newtonsoft.Json;

namespace Game.Controllers
{
    public static class PlayerController
	{
		public static void LoadPlayer(RPPlayer player)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			var pos = PositionService.Get(player.PositionId);
			if (pos == null) return;

			player.SetDimension(0);
			player.LoggedIn = true;
			player.AdminRank = account.AdminRank;
			player.Hunger = account.Hunger;
			player.Thirst = account.Thirst;
			player.Alive = account.Alive;
			player.Coma = account.Coma;
			player.Stabilized = account.Stabilized;
			player.Laptop = account.Laptop;
			player.Phone = account.Phone;
			player.PhoneNumber = account.PhoneNumber;
			player.Jailtime = account.Jailtime;
			player.Level = account.Level;
			player.Xp = account.Xp;
			player.XpTicks = account.XpTicks;
			player.TeamDuty = account.TeamDuty;
			player.IsInHospital = account.IsInHospital;
			player.VestItemId = account.ArmorItemId;
			player.SWATDuty = account.SWATDuty;
			player.DamageCap = account.DamageCap;

			player.ApplyTeam();
			player.ApplyAdmin();

			player.Spawn(player.Position, 0);
			player.SetPosition(pos.Position);
			player.SetHealth(account.Health);
			player.SetArmor(account.Armor);
			player.SetInvincible(false);
			player.Rotation = pos.Rotation;

			ApplyPlayerCustomization(player);
			ApplyPlayerTattoos(player);
			ApplyPlayerClothes(player);
			ApplyPlayerLoadout(player);
			player.ShowComponent("Hud", true);
			player.EmitBrowser("Hud:ShowInfo", true, JsonConvert.SerializeObject(new HudClientData(account.Money, account.Hunger, account.Thirst)));

			if (!account.Alive)
				SetPlayerDead(player, account.InjuryType);

			if (account.Cuffed)
				SetPlayerCuffed(player, true);

			if (account.Roped)
				SetPlayerRoped(player, true);

			if (player.IsInHospital)
				player.Frozen = true;

			player.Emit("Client:DoorModule:Init", DoorModule.JSONData);
			player.Emit("Client:PlayerModule:SetSuperSecretFeature", player.DamageCap);
			player.SetStreamSyncedMetaData("PLAYER_NAME", player.Name);

			VoiceModule.ConnectToVoice(player);

			account.LastOnline = DateTime.Now;
			AccountService.Update(account);

			if(account.SupportCallMessage.Length > 0)
			{
				AdminController.BroadcastTeam("Support Aufruf", $"{player.Name} hat sich eingeloggt! Offener Supportaufruf: {account.SupportCallMessage}", NotificationType.WARN);
			}
		}

		public static void ApplyPlayerCustomization(RPPlayer player)
		{
			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			player.Model = custom.Gender ? 1885233650 : 2627665880;

			player.SetHeadBlendData(
				(uint)custom.Mother,
				(uint)custom.Father,
				0,
				(uint)custom.Mother,
				(uint)custom.Father,
				0,
				custom.ShapeSimilarity,
				custom.SkinSimilarity,
				0);

			player.SetClothing(2, custom.Hair, 0, 0);
			player.HairColor = (byte)custom.HairColor;
			player.HairHighlightColor = (byte)custom.HairHighlightColor;

			if (custom.Beard > -1)
			{
				player.SetHeadOverlay(1, (byte)custom.Beard, custom.BeardOpacity);
				player.SetHeadOverlayColor(1, 1, (byte)custom.BeardColor, (byte)custom.BeardColor);
			}
			else
			{
				player.RemoveHeadOverlay(1);
			}

			if (custom.Eyebrow > -1)
			{
				player.SetHeadOverlay(2, (byte)custom.Eyebrow, 1);
				player.SetHeadOverlayColor(2, 1, (byte)custom.EyebrowColor, (byte)custom.EyebrowColor);
			}
			else
			{
				player.RemoveHeadOverlay(2);
			}

			if (custom.Age > -1)
			{
				player.SetHeadOverlay(3, (byte)custom.Age, 1);
			}
			else
			{
				player.RemoveHeadOverlay(3);
			}

			if(custom.Makeup > -1)
			{
				player.SetHeadOverlay(4, (byte)custom.Makeup, custom.MakeupOpacity);
				player.SetHeadOverlayColor(4, 2, (byte)custom.MakeupColor, (byte)custom.MakeupColor);
			}
			else
			{
				player.RemoveHeadOverlay(4);
			}

			if (custom.Blush > -1)
			{
				player.SetHeadOverlay(5, (byte)custom.Blush, custom.BlushOpacity);
				player.SetHeadOverlayColor(5, 1, (byte)custom.BlushColor, (byte)custom.BlushColor);
			}
			else
			{
				player.RemoveHeadOverlay(5);
			}

			if (custom.Lipstick > -1)
			{
				player.SetHeadOverlay(8, (byte)custom.Lipstick, custom.LipstickOpacity);
				player.SetHeadOverlayColor(8, 1, (byte)custom.LipstickColor, (byte)custom.LipstickColor);
			}
			else
			{
				player.RemoveHeadOverlay(8);
			}

			player.SetEyeColor((byte)custom.EyeColor);

			player.SetFaceFeature(0, custom.NoseWidth);
			player.SetFaceFeature(1, custom.NosePeak);
			player.SetFaceFeature(2, custom.NoseLength);
			player.SetFaceFeature(3, custom.NoseBridge);
			player.SetFaceFeature(4, custom.NoseHeight);
			player.SetFaceFeature(5, custom.NoseMovement);
			player.SetFaceFeature(12, custom.LipWidth);
			player.SetFaceFeature(19, custom.NeckWidth);
		}

		public static void ApplyPlayerTattoos(RPPlayer player)
		{
			player.ClearDecorations();

			var tattoos = TattooService.GetFromAccount(player.DbId);

			foreach(var tattoo in tattoos)
				player.AddDecoration(tattoo.Collection, tattoo.Overlay);
		}

		public static void ApplyPlayerClothes(RPPlayer player, bool armor = true)
		{
			var clothes = ClothesService.Get(player.TempClothesId > 0 ? player.TempClothesId : player.ClothesId);
			if (clothes == null) return;

			if(player.ClothesId == clothes.Id && player.Armor <= 0 && clothes.Armor > 0)
			{
				clothes.Armor = 0;
				clothes.ArmorColor = 0;
				clothes.ArmorDlc = 0;
				ClothesService.Update(clothes);
			}

			player.SetClothing(1, clothes.Mask, clothes.MaskColor, clothes.MaskDlc);
			player.SetClothing(3, clothes.Body, clothes.BodyColor, clothes.BodyDlc);
			player.SetClothing(4, clothes.Pants, clothes.PantsColor, clothes.PantsDlc);
			player.SetClothing(6, clothes.Shoes, clothes.ShoesColor, clothes.ShoesDlc);
			player.SetClothing(7, clothes.Accessories, clothes.AccessoriesColor, clothes.AccessoriesDlc);
			player.SetClothing(8, clothes.Undershirt, clothes.UndershirtColor, clothes.UndershirtDlc);
			if(armor) player.SetClothing(9, clothes.Armor, clothes.ArmorColor, clothes.ArmorDlc);
			player.SetClothing(10, clothes.Decals, clothes.DecalsColor, clothes.DecalsDlc);
			player.SetClothing(11, clothes.Top, clothes.TopColor, clothes.TopDlc);

			player.SetProp(0, clothes.Hat, clothes.HatColor, clothes.HatDlc);
			player.SetProp(1, clothes.Glasses, clothes.GlassesColor, clothes.GlassesDlc);
			player.SetProp(2, clothes.Ears, clothes.EarsColor, clothes.EarsDlc);
			player.SetProp(6, clothes.Watch, clothes.WatchColor, clothes.WatchDlc);
			player.SetProp(7, clothes.Bracelet, clothes.BraceletColor, clothes.BraceletDlc);
		}

		public static void ApplyPlayerClothes(RPPlayer player, ClothesModel clothes)
		{
			player.SetClothing(1, clothes.Mask, clothes.MaskColor, clothes.MaskDlc);
			player.SetClothing(3, clothes.Body, clothes.BodyColor, clothes.BodyDlc);
			player.SetClothing(4, clothes.Pants, clothes.PantsColor, clothes.PantsDlc);
			player.SetClothing(6, clothes.Shoes, clothes.ShoesColor, clothes.ShoesDlc);
			player.SetClothing(7, clothes.Accessories, clothes.AccessoriesColor, clothes.AccessoriesDlc);
			player.SetClothing(8, clothes.Undershirt, clothes.UndershirtColor, clothes.UndershirtDlc);
			player.SetClothing(9, clothes.Armor, clothes.ArmorColor, clothes.ArmorDlc);
			player.SetClothing(10, clothes.Decals, clothes.DecalsColor, clothes.DecalsDlc);
			player.SetClothing(11, clothes.Top, clothes.TopColor, clothes.TopDlc);

			player.SetProp(0, clothes.Hat, clothes.HatColor, clothes.HatDlc);
			player.SetProp(1, clothes.Glasses, clothes.GlassesColor, clothes.GlassesDlc);
			player.SetProp(2, clothes.Ears, clothes.EarsColor, clothes.EarsDlc);
			player.SetProp(6, clothes.Watch, clothes.WatchColor, clothes.WatchDlc);
			player.SetProp(7, clothes.Bracelet, clothes.BraceletColor, clothes.BraceletDlc);
		}

		public static void ApplyPlayerLoadout(RPPlayer player)
		{
			player.RemoveAllWeapons(true);
			player.Weapons.Clear();

			var weapons = LoadoutService.GetPlayerLoadout(player.DbId);

			foreach(var weapon in weapons)
			{
				var attatchments = LoadoutService.GetLoadoutAttatchments(weapon.Id);

				player.AddWeapon(weapon.Hash, weapon.Ammo, false, (byte)weapon.TintIndex, attatchments.Select(x => x.Hash).ToList());
				player.SetWeaponTintIndex(weapon.Hash, (byte)weapon.TintIndex);

				foreach(var attatchment in attatchments)
				{
					player.AddAttatchment(weapon.Hash, attatchment.Hash);
				}
			}
		}

		public static void SetPlayerAlive(RPPlayer player, bool coma)
		{
			if (coma)
			{
				var account = AccountService.Get(player.DbId);
				if (account == null) return;

				account.Backpack = false;
				PlayerController.SetMoney(player, 0);
				InventoryService.ClearInventoryItems(player.InventoryId);
				LoadoutService.ClearPlayerLoadout(player.DbId);
				player.RemoveAllWeapons(true);
				player.Weapons.Clear();

				AccountService.Update(account);
			}

			player.Alive = true;
			player.InjuryType = 0;
			player.Coma = false;
			player.SetInvincible(false);
			player.IsInHospital = false;
			player.SetStreamSyncedMetaData("ALIVE", true);
			player.SetStreamSyncedMetaData("STABILIZED", false);
			player.StopAnimation();
			VoiceModule.OnPlayerAliveChange(player, true);
		}

		public static void SetPlayerDead(RPPlayer player, InjuryType injury, bool fall = true)
		{
			player.DeathTime = DateTime.Now;
			player.Alive = false;
			player.InjuryType = injury;
			player.Coma = false;
			player.IsInHospital = false;
			player.SetStreamSyncedMetaData("ALIVE", false);
			player.SetStreamSyncedMetaData("STABILIZED", false);
			SetPlayerCuffed(player, false);
			SetPlayerRoped(player, false);
			if (player.RadioTalking) VoiceModule.RadioTalkingState(player, false);
			VoiceModule.EnableRadio(player, false);

			if (player.IsFarming)
			{
				FarmingController.StopFarming(player);
			}

			if (fall)
			{
				Task.Run(async () =>
				{
					await Task.Delay(2000);

					player.Spawn(player.Position, 0);
					player.Emit("Client:AnticheatModule:SetHealth", 200);
					if (!player.Alive) player.SetInvincible(true);
				});
				return;
			}

			player.Spawn(player.Position, 0);
			player.Emit("Client:AnticheatModule:SetHealth", 200);
			if (!player.Alive) player.SetInvincible(true);
			VoiceModule.OnPlayerAliveChange(player, false);
		}

		public static void SetPlayerCuffed(RPPlayer player, bool state)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if (player.IsFarming)
			{
				FarmingController.StopFarming(player);
			}

			if (state) player.PlayAnimation(AnimationType.CUFFED);
			else player.StopAnimation();

			player.SetStreamSyncedMetaData("CUFFED", state);
			player.HasBeenSearched = false;

			account.Cuffed = state;
			AccountService.Update(account);
		}

		public static void SetPlayerRoped(RPPlayer player, bool state)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			if (player.IsFarming)
			{
				FarmingController.StopFarming(player);
			}

			if (state) player.PlayAnimation(AnimationType.ROPED);
			else player.StopAnimation();

			player.SetStreamSyncedMetaData("ROPED", state);
			player.HasBeenSearched = false;

			account.Roped = state;
			AccountService.Update(account);
		}

		public static void SetMoney(RPPlayer player, int money)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.Money = money;
			AccountService.Update(account);

			player.EmitBrowser("Hud:SetMoney", money);
		}

		public static void AddMoney(RPPlayer player, int amount)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.Money += amount;
			AccountService.Update(account);

			player.EmitBrowser("Hud:SetMoney", account.Money);
		}

		public static void RemoveMoney(RPPlayer player, int amount)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.Money -= amount;
			AccountService.Update(account);

			player.EmitBrowser("Hud:SetMoney", account.Money);
		}

		public static void SetClothes(RPPlayer player, ClothesModel clothes, int component, int drawable, int texture, uint dlc, bool apply = false, bool update = false)
		{
			switch (component)
			{
				case 1:
					clothes.Mask = drawable;
					clothes.MaskColor = texture;
					clothes.MaskDlc = dlc;
					break;
				case 3:
					clothes.Body = drawable;
					clothes.BodyColor = texture;
					clothes.BodyDlc = dlc;
					break;
				case 4:
					clothes.Pants = drawable;
					clothes.PantsColor = texture;
					clothes.PantsDlc = dlc;
					break;
				case 6:
					clothes.Shoes = drawable;
					clothes.ShoesColor = texture;
					clothes.ShoesDlc = dlc;
					break;
				case 7:
					clothes.Accessories = drawable;
					clothes.AccessoriesColor = texture;
					clothes.AccessoriesDlc = dlc;
					break;
				case 8:
					clothes.Undershirt = drawable;
					clothes.UndershirtColor = texture;
					clothes.UndershirtDlc = dlc;
					break;
				case 9:
					clothes.Armor = drawable;
					clothes.ArmorColor = texture;
					clothes.ArmorDlc = dlc;
					break;
				case 10:
					clothes.Decals = drawable;
					clothes.DecalsColor = texture;
					clothes.DecalsDlc = dlc;
					break;
				case 11:
					clothes.Top = drawable;
					clothes.TopColor = texture;
					clothes.TopDlc = dlc;
					break;
			}

			if (apply)
			{
				player.SetClothing(component, drawable, texture, dlc);
			}

			if (update)
			{
				ClothesService.Update(clothes);
			}
		}

		public static void SetClothes(RPPlayer player, int component, int drawable, int texture, uint dlc, bool apply = false, bool update = false)
		{
			var clothes = ClothesService.Get(player.ClothesId);
			if (clothes == null) return;

			SetClothes(player, clothes, component, drawable, texture, dlc, apply, update);
		}

		public static void SetClothes(RPPlayer player, ClothesShopItemModel model, bool apply = false, bool update = false)
		{
			SetClothes(player, model.Component, model.Drawable, model.Texture, model.Dlc, apply, update);
		}

		public static void SetProps(RPPlayer player, ClothesModel clothes, int component, int drawable, int texture, uint dlc, bool apply = false, bool update = false)
		{
			switch (component)
			{
				case 0:
					clothes.Hat = drawable;
					clothes.HatColor = texture;
					clothes.HatDlc = dlc;
					break;
				case 1:
					clothes.Glasses = drawable;
					clothes.GlassesColor = texture;
					clothes.GlassesDlc = dlc;
					break;
				case 2:
					clothes.Ears = drawable;
					clothes.EarsColor = texture;
					clothes.EarsDlc = dlc;
					break;
				case 6:
					clothes.Watch = drawable;
					clothes.WatchColor = texture;
					clothes.WatchDlc = dlc;
					break;
				case 7:
					clothes.Bracelet = drawable;
					clothes.BraceletColor = texture;
					clothes.BraceletDlc = dlc;
					break;
			}

			if (apply)
			{
				player.SetProp(component, drawable, texture, dlc);
			}

			if (update)
			{
				ClothesService.Update(clothes);
			}
		}

		public static void SetProps(RPPlayer player, int component, int drawable, int texture, uint dlc, bool apply = false, bool update = false)
		{
			var clothes = ClothesService.Get(player.ClothesId);
			if (clothes == null) return;

			SetProps(player, clothes, component, drawable, texture, dlc, apply, update);
		}

		public static void SetProps(RPPlayer player, ClothesShopItemModel model, bool apply = false, bool update = false)
		{
			SetProps(player, model.Component, model.Drawable, model.Texture, model.Dlc, apply, update);
		}

		public static void SetPlayerTeam(RPPlayer player, int team, int rank, bool admin, bool storage, bool bank)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			player.TeamId = team;
			player.ApplyTeam();

			account.TeamId = team;
			account.TeamRank = rank;
			account.TeamAdmin = admin;
			account.TeamStorage = storage;
			account.TeamBank = bank;
			AccountService.Update(account);
		}

		public static void AnticheatBanPlayer(RPPlayer player, DateTime until, string reason)
		{
			var account = AccountService.Get(player.DbId);
			if (account == null) return;

			account.BannedUntil = until;
			account.BanReason = "Cheating";
			AccountService.Update(account);

			LogService.LogPlayerBan(player.DbId, 0, reason);
			player.Kick("Du wurdest gebannt! Grund: Cheating");

			AccountService.AddAdminHistory(new(account.Id, reason, 0, "Anticheat", DateTime.Now, AdminHistoryType.BAN));
		}
	}
}