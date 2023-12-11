using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Models.NativeMenu;
using Database.Services;
using Newtonsoft.Json;

namespace Game.Modules
{
	public static class NMenuModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:NMenu:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:NMenu:PlayAnimation", PlayAnimation);
			Alt.OnClient<RPPlayer>("Server:NMenu:OpenSlotSelector", OpenSlotSelector);
			Alt.OnClient<RPPlayer, int>("Server:NMenu:OpenCategorySelector", OpenCategorySelector);
			Alt.OnClient<RPPlayer, int, int>("Server:NMenu:OpenAnimationSelector", OpenAnimationSelector);
			Alt.OnClient<RPPlayer, int, int>("Server:NMenu:SaveAnimation", SaveAnimation);
		}

		private static void Open(RPPlayer player)
		{
			if (!player.LoggedIn) return;

			var items = new List<object>();
			foreach (var favorite in AnimationService.GetFavorites(player.DbId))
			{
				var anim = AnimationService.Get(favorite.AnimationId);
				if(anim == null) continue;

				items.Add(new { anim.Id, anim.Label });
			}

			player.ShowComponent("NMenu", true, JsonConvert.SerializeObject(items));
		}

		private static void PlayAnimation(RPPlayer player, int animationId)
		{
			if (!player.LoggedIn || animationId < -1) return;

			if(animationId == -1)
			{
				player.StopAnimation();
				return;
			}

			if (animationId == 0)
			{
				OpenSlotSelector(player);
				return;
			}

			var anim = AnimationService.Get(animationId);
			if (anim == null) return;

			player.StopAnimation();
			player.PlayAnimation(anim.Dictionary, anim.Name, anim.Flags);
		}

		private static void OpenSlotSelector(RPPlayer player)
		{
			var items = new List<NativeMenuItem>()
			{
				new($"Slot 1 belegen", false, "Server:NMenu:OpenCategorySelector", 0),
				new($"Slot 2 belegen", false, "Server:NMenu:OpenCategorySelector", 1),
				new($"Slot 3 belegen", false, "Server:NMenu:OpenCategorySelector", 2),
				new($"Slot 4 belegen", false, "Server:NMenu:OpenCategorySelector", 3),
				new($"Slot 5 belegen", false, "Server:NMenu:OpenCategorySelector", 4),
				new($"Slot 6 belegen", false, "Server:NMenu:OpenCategorySelector", 5),
				new($"Slot 7 belegen", false, "Server:NMenu:OpenCategorySelector", 6),
				new($"Slot 8 belegen", false, "Server:NMenu:OpenCategorySelector", 7),
				new($"Slot 9 belegen", false, "Server:NMenu:OpenCategorySelector", 8),
				new($"Slot 10 belegen", false, "Server:NMenu:OpenCategorySelector", 9)
			};

			player.ShowNativeMenu(true, new("Schnellauswahl belegen", items));
		}

		private static void OpenCategorySelector(RPPlayer player, int slot)
		{
			var items = new List<NativeMenuItem>()
			{
				new("Zurück", true, "Server:NMenu:OpenSlotSelector")
			};

			foreach(var category in AnimationService.GetCategories())
			{
				items.Add(new(category.Label, false, "Server:NMenu:OpenAnimationSelector", slot, category.Id));
			}

			player.ShowNativeMenu(true, new("Schnellauswahl belegen", items));
		}

		private static void OpenAnimationSelector(RPPlayer player, int slot, int category)
		{
			var items = new List<NativeMenuItem>()
			{
				new("Zurück", true, "Server:NMenu:OpenCategorySelector")
			};

			foreach (var animation in AnimationService.GetFromCategory(category))
			{
				items.Add(new(animation.Label, true, "Server:NMenu:SaveAnimation", slot, animation.Id));
			}

			player.ShowNativeMenu(true, new("Schnellauswahl belegen", items));
		}

		private static void SaveAnimation(RPPlayer player, int slot, int animationId)
		{
			if (!player.LoggedIn || animationId < 1 || slot < 0 || slot > 9) return;

			var item = AnimationService.GetFavoriteBySlot(player.DbId, slot);
			if(item != null)
			{
				item.AnimationId = animationId;
				AnimationService.UpdateFavorite(item);
				return;
			}

			AnimationService.AddFavorite(new(player.DbId, animationId, slot));
		}
	}
}