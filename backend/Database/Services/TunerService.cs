using Database.Models.Bank;
using Database.Models.Tuner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Services
{
	public static class TunerService
	{
		public static List<TunerModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Tuner.ToList();
		}

		public static void Add(TunerModel model)
		{
			using var ctx = new Context();
			ctx.Tuner.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(TunerModel model)
		{
			using var ctx = new Context();
			ctx.Tuner.Remove(model);
			ctx.SaveChanges();
		}

		// categories
		public static List<TunerCategoryModel> GetAllCategories()
		{
			using var ctx = new Context();
			return ctx.TunerCategories.ToList();
		}

		public static void AddCategory(TunerCategoryModel model)
		{
			using var ctx = new Context();
			ctx.TunerCategories.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveCategory(TunerCategoryModel model)
		{
			using var ctx = new Context();
			ctx.TunerCategories.Remove(model);
			ctx.SaveChanges();
		}

		public static TunerCategoryModel? GetCategory(int id)
		{
			using var ctx = new Context();
			return ctx.TunerCategories.FirstOrDefault(x => x.Id == id);
		}

		// items
		public static List<TunerItemModel> GetItems(int category)
		{
			using var ctx = new Context();
			return ctx.TunerItems.Where(x => x.CategoryId == category).ToList();
		}

		public static void AddItem(TunerItemModel model)
		{
			using var ctx = new Context();
			ctx.TunerItems.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveItem(TunerItemModel model)
		{
			using var ctx = new Context();
			ctx.TunerItems.Remove(model);
			ctx.SaveChanges();
		}

		public static TunerItemModel? GetItem(int id)
		{
			using var ctx = new Context();
			return ctx.TunerItems.FirstOrDefault(x => x.Id == id);
		}
	}
}
