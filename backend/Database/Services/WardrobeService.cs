using Core.Enums;
using Database.Models.Wardrobe;

namespace Database.Services
{
    public static class WardrobeService
	{
		public static List<WardrobeModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Wardrobes.ToList();
		}

		public static void Add(WardrobeModel model)
		{
			using var ctx = new Context();
			ctx.Wardrobes.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(WardrobeModel model)
		{
			using var ctx = new Context();
			ctx.Wardrobes.Remove(model);
			ctx.SaveChanges();
		}

		public static WardrobeModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Wardrobes.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(WardrobeModel model)
		{
			using var ctx = new Context();
			ctx.Wardrobes.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<WardrobeModel> models)
		{
			using var ctx = new Context();
			ctx.Wardrobes.UpdateRange(models);
			ctx.SaveChanges();
		}

		// outfits
		public static List<OutfitModel> GetPlayerOutfits(int accountId)
		{
			using var ctx = new Context();
			return ctx.Outfits.Where(x => x.AccountId == accountId).ToList();
		}

		public static void AddOutfit(OutfitModel model)
		{
			using var ctx = new Context();
			ctx.Outfits.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveOutfit(OutfitModel model)
		{
			using var ctx = new Context();
			ctx.Outfits.Remove(model);
			ctx.SaveChanges();
		}

		public static OutfitModel? GetOutfit(int id)
		{
			using var ctx = new Context();
			return ctx.Outfits.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateOutfit(OutfitModel model)
		{
			using var ctx = new Context();
			ctx.Outfits.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateOutfits(IEnumerable<OutfitModel> models)
		{
			using var ctx = new Context();
			ctx.Outfits.UpdateRange(models);
			ctx.SaveChanges();
		}

		// items

		public static List<WardrobeItemModel> GetItemsFromOwner(int component, bool prop, int accountId, int teamId, int gender)
		{
			using var ctx = new Context();
			return ctx.WardrobeItems.Where(x => (x.Component == component && x.Prop == prop && (x.OwnerId == 0 || x.OwnerType == OwnerType.PLAYER && x.OwnerId == accountId || x.OwnerType == OwnerType.TEAM && x.OwnerId == teamId)) && (x.Gender == gender || x.Gender == 2)).ToList();
		}

		public static List<WardrobeItemModel> GetItemsFromOwner(int accountId, int teamId, int gender)
		{
			using var ctx = new Context();
			return ctx.WardrobeItems.Where(x => (x.OwnerId == 0 || x.OwnerType == OwnerType.PLAYER && x.OwnerId == accountId || x.OwnerType == OwnerType.TEAM && x.OwnerId == teamId) && (x.Gender == gender || x.Gender == 2)).ToList();
		}

		public static void AddItem(WardrobeItemModel model)
		{
			using var ctx = new Context();
			ctx.WardrobeItems.Add(model);
			ctx.SaveChanges();
		}

		public static void AddItems(IEnumerable<WardrobeItemModel> models)
		{
			using var ctx = new Context();
			ctx.WardrobeItems.AddRange(models);
			ctx.SaveChanges();
		}

		public static void RemoveItem(WardrobeItemModel model)
		{
			using var ctx = new Context();
			ctx.WardrobeItems.Remove(model);
			ctx.SaveChanges();
		}

		public static WardrobeItemModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.WardrobeItems.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateItem(WardrobeItemModel model)
		{
			using var ctx = new Context();
			ctx.WardrobeItems.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateItem(IEnumerable<WardrobeItemModel> models)
		{
			using var ctx = new Context();
			ctx.WardrobeItems.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}