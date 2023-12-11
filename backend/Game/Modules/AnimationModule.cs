using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Core.Models.NativeMenu;
using Database.Services;

namespace Game.Modules
{
	public static class AnimationModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:Animation:Open", Open);
			Alt.OnClient<RPPlayer, int>("Server:Animation:OpenCategory", OpenCategory);
			Alt.OnClient<RPPlayer, int>("Server:Animation:Play", PlayAnimation);
		}

		private static void Open(RPPlayer player)
		{
			if (!player.LoggedIn) return;

			var items = new List<NativeMenuItem>();
			foreach(var item in AnimationService.GetCategories())
			{
				items.Add(new(item.Label, false, "Server:Animation:OpenCategory", item.Id));
			}

			player.ShowNativeMenu(true, new("Animationen", items));
		}

		private static void OpenCategory(RPPlayer player, int category)
		{
			if (!player.LoggedIn) return;

			var items = new List<NativeMenuItem>();
			foreach (var item in AnimationService.GetFromCategory(category))
			{
				items.Add(new(item.Label, false, "Server:Animation:Play", item.Id));
			}

			player.ShowNativeMenu(true, new("Animationen", items));
		}

		private static void PlayAnimation(RPPlayer player, int animationId)
		{
			if (!player.LoggedIn || player.IsInVehicle) return;

			var animation = AnimationService.Get(animationId);
			if (animation == null) return;

			player.PlayAnimation(animation.Dictionary, animation.Name, animation.Flags);
		}
	}
}