using Core.Attribute;
using Core.Entities;
using Database.Models.Tuner;
using Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Commands.Gamedesign
{
	public static class TunerCommands
	{
		[Command("addtunercategory")]
		public static void AddTunerCategory(RPPlayer player, string name)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var model = new TunerCategoryModel(name.Replace('_', ' '));
			TunerService.AddCategory(model);
			player.Notify("Information", $"Du hast eine Kategorie erstellt! ID: {model.Id}", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("addtuneritem")]
		public static void AddTunerItem(RPPlayer player, int category, string name, int modcategory, int modvalue)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var model = new TunerItemModel(category, name.Replace('_', ' '), modcategory, modvalue);
			TunerService.AddItem(model);
			player.Notify("Information", $"Du hast ein Item erstellt!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
