using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Core.Models.NativeMenu;
using Database.Models.Account;
using Database.Models.Wardrobe;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules
{
    public static class WardrobeModule
	{
		[Initialize]
		public static void Initialize()
		{
			foreach(var wardrobe in WardrobeService.GetAll())
				WardrobeController.LoadWardrobe(wardrobe);

			Alt.OnClient<RPPlayer>("Server:Wardrobe:Open", Open);
			Alt.OnClient<RPPlayer, int, bool>("Server:Wardrobe:SelectCategory", SelectCategory);
			Alt.OnClient<RPPlayer, int>("Server:Wardrobe:SelectItem", SelectItem);
			Alt.OnClient<RPPlayer>("Server:Wardrobe:ShowOutfits", ShowOutfits);
			Alt.OnClient<RPPlayer, int>("Server:Wardrobe:SelectOutfit", SelectOutfit);
			Alt.OnClient<RPPlayer>("Server:Wardrobe:AddOutfit", AddOutfit);
			Alt.OnClient<RPPlayer, string>("Server:Wardrobe:CreateOutfit", CreateOutfit);
			Alt.OnClient<RPPlayer, int>("Server:Wardrobe:ApplyOutfit", ApplyOutfit);
			Alt.OnClient<RPPlayer, int>("Server:Wardrobe:DeleteOutfit", DeleteOutfit);
		}

		private static void Open(RPPlayer player)
		{
			if (player.AdminDuty) return;

			var shape = RPShape.All.FirstOrDefault(x => x.Dimension == player.Dimension && x.ShapeType == ColshapeType.WARDROBE && x.Position.Distance(player.Position) <= x.Size);
			if (shape == null) return;

			var model = WardrobeService.Get(shape.ShapeId);
			if (model == null) return;

			if (model.OwnerId > 0 && (model.OwnerType == OwnerType.PLAYER && player.DbId != model.OwnerId || model.OwnerType == OwnerType.TEAM && player.TeamId != model.OwnerId || model.OwnerType == OwnerType.SWAT && !player.SWATDuty)) return;

			player.ShowNativeMenu(true, new("Kleiderschrank", new()
			{
				new("Outfits", false, "Server:Wardrobe:ShowOutfits"),
				new("Maske", false, "Server:Wardrobe:SelectCategory", 1, false),
				new("Oberteile", false, "Server:Wardrobe:SelectCategory", 11, false),
				new("Unterteile", false, "Server:Wardrobe:SelectCategory", 8, false),
				new("Körper", false, "Server:Wardrobe:SelectCategory", 3, false),
				new("Hosen", false, "Server:Wardrobe:SelectCategory", 4, false),
				new("Schuhe", false, "Server:Wardrobe:SelectCategory", 6, false),
				new("Accessories", false, "Server:Wardrobe:SelectCategory", 7, false),
				new("Decals", false, "Server:Wardrobe:SelectCategory", 10, false),
				new("Rucksäcke", false, "Server:Wardrobe:SelectCategory", 5, false),

				new("Hüte", false, "Server:Wardrobe:SelectCategory", 0, true),
				new("Brillen", false, "Server:Wardrobe:SelectCategory", 1, true),
				new("Ohrringe", false, "Server:Wardrobe:SelectCategory", 2, true),
				new("Uhren", false, "Server:Wardrobe:SelectCategory", 6, true),
				new("Armbänder", false, "Server:Wardrobe:SelectCategory", 7, true)
			}));
		}

		private static void SelectCategory(RPPlayer player, int component, bool prop)
		{
			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			var items = WardrobeService.GetItemsFromOwner(component, prop, player.DbId, player.TeamId, custom.Gender ? 1 : 0);
			var nativeItems = new List<NativeMenuItem>();

			foreach (var item in items)
			{
				nativeItems.Add(new(item.Label, false, "Server:Wardrobe:SelectItem", item.Id));
			}

			player.ShowNativeMenu(true, new("Kleiderschrank", nativeItems));
		}

		private static void SelectItem(RPPlayer player, int itemId)
		{
			var item = WardrobeService.GetItem(itemId);
			if (item == null || !OwnsClothing(player, item)) return;

			if (item.Prop)
			{
				PlayerController.SetProps(player, item.Component, item.Drawable, item.Texture, item.Dlc, true, true);
			}
			else
			{
				PlayerController.SetClothes(player, item.Component, item.Drawable, item.Texture, item.Dlc, true, true);
			}
		}

		private static void ShowOutfits(RPPlayer player)
		{
			var outfits = WardrobeService.GetPlayerOutfits(player.DbId);
			var nativeItems = new List<NativeMenuItem>()
			{
				new("Outfit hinzufügen", true, "Server:Wardrobe:AddOutfit")
			};

			foreach(var outfit in outfits)
				nativeItems.Add(new(outfit.Name, false, "Server:Wardrobe:SelectOutfit", outfit.Id));

			player.ShowNativeMenu(true, new("Outfits", nativeItems));
		}

		private static void SelectOutfit(RPPlayer player, int outfitId)
		{
			var outfit = WardrobeService.GetOutfit(outfitId);
			if (outfit == null || outfit.AccountId != player.DbId) return;

			player.ShowNativeMenu(true, new(outfit.Name, new()
			{
				new("Outfit Anziehen", true, "Server:Wardrobe:ApplyOutfit", outfit.Id),
				new("Outfit Löschen", true, "Server:Wardrobe:DeleteOutfit", outfit.Id),
			}));
		}

		private static void AddOutfit(RPPlayer player)
		{
			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Outfit erstellen",
				Message = "Gebe den Namen des Outfits an",
				Type = (int)InputType.TEXT,
				CallbackEvent = "Server:Wardrobe:CreateOutfit",
				CallbackArgs = new List<object>()
			}));
		}

		private static void CreateOutfit(RPPlayer player, string name)
		{
			var currentCothes = ClothesService.Get(player.ClothesId);
			if (currentCothes == null) return;

			var clothes = new ClothesModel();
			currentCothes.CopyTo(clothes);
			clothes.Armor = 0;
			clothes.ArmorColor = 0;
			clothes.ArmorDlc = 0;
			ClothesService.Add(clothes);

			var outfit = new OutfitModel(name, player.DbId, clothes.Id);
			WardrobeService.AddOutfit(outfit);
			player.Notify("Information", "Du hast ein neues Outfit erstellt!", NotificationType.SUCCESS);
		}

		private static void ApplyOutfit(RPPlayer player, int outfitId)
		{
			var outfit = WardrobeService.GetOutfit(outfitId);
			if (outfit == null || outfit.AccountId != player.DbId) return;

			var clothes = ClothesService.Get(player.ClothesId);
			if (clothes == null) return;

			var outfitClothes = ClothesService.Get(outfit.ClothesId);
			if (outfitClothes == null) return;

			outfitClothes.CopyTo(clothes);

			ClothesService.Update(clothes);
			PlayerController.ApplyPlayerClothes(player, clothes);
			player.Notify("Information", $"Du hast Outfit {outfit.Name} angezogen!", Core.Enums.NotificationType.SUCCESS);
		}

		private static void DeleteOutfit(RPPlayer player, int outfitId)
		{
			var outfit = WardrobeService.GetOutfit(outfitId);
			if (outfit == null || outfit.AccountId != player.DbId) return;

			WardrobeService.RemoveOutfit(outfit);
			player.Notify("Information", $"Du hast Outfit {outfit.Name} gelöscht!", Core.Enums.NotificationType.SUCCESS);
		}

		private static bool OwnsClothing(RPPlayer player, WardrobeItemModel item)
		{
			return item.OwnerId == 0 || (item.OwnerType == Core.Enums.OwnerType.PLAYER && item.OwnerId == player.DbId) || (item.OwnerType == Core.Enums.OwnerType.TEAM && player.TeamId == item.OwnerId);
		}
	}
}