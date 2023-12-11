using Database.Models.Animation;

namespace Database.Services
{
    public static class AnimationService
	{
		// AnimationModel
		public static void Add(AnimationModel model)
		{
			using var ctx = new Context();
			ctx.Animations.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(AnimationModel model)
		{
			using var ctx = new Context();
			ctx.Animations.Remove(model);
			ctx.SaveChanges();
		}

		public static AnimationModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Animations.FirstOrDefault(x => x.Id == id);
		}

		public static List<AnimationModel> GetFromCategory(int category)
		{
			using var ctx = new Context();
			return ctx.Animations.Where(x => x.CategoryId == category).ToList();
		}

		public static void Update(AnimationModel model)
		{
			using var ctx = new Context();
			ctx.Animations.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<AnimationModel> models)
		{
			using var ctx = new Context();
			ctx.Animations.UpdateRange(models);
			ctx.SaveChanges();
		}

		// AnimationCategoryModel
		public static List<AnimationCategoryModel> GetCategories()
		{
			using var ctx = new Context();
			return ctx.AnimationCategories.ToList();
		}

		public static void AddCategory(AnimationCategoryModel model)
		{
			using var ctx = new Context();
			ctx.AnimationCategories.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveCategory(AnimationCategoryModel model)
		{
			using var ctx = new Context();
			ctx.AnimationCategories.Remove(model);
			ctx.SaveChanges();
		}

		// AnimationFavoriteModel
		public static void AddFavorite(AnimationFavoriteModel model)
		{
			using var ctx = new Context();
			ctx.AnimationFavorites.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveFavorite(AnimationFavoriteModel model)
		{
			using var ctx = new Context();
			ctx.AnimationFavorites.Remove(model);
			ctx.SaveChanges();
		}

		public static AnimationFavoriteModel? GetFavorite(int id)
		{
			using var ctx = new Context();
			return ctx.AnimationFavorites.FirstOrDefault(x => x.Id == id);
		}

		public static AnimationFavoriteModel? GetFavoriteBySlot(int accountId, int slot)
		{
			using var ctx = new Context();
			return ctx.AnimationFavorites.FirstOrDefault(x => x.AccountId == accountId && x.Slot == slot);
		}

		public static List<AnimationFavoriteModel> GetFavorites(int id)
		{
			using var ctx = new Context();
			return ctx.AnimationFavorites.Where(x => x.AccountId == id).ToList();
		}

		public static void UpdateFavorite(AnimationFavoriteModel model)
		{
			using var ctx = new Context();
			ctx.AnimationFavorites.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateFavorite(IEnumerable<AnimationFavoriteModel> models)
		{
			using var ctx = new Context();
			ctx.AnimationFavorites.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}