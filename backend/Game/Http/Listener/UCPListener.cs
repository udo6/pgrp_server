using Core.Enums;
using Database.Models.Account;
using Database.Models.Inventory;
using Database.Models;
using Database.Services;
using Game.Http.Utils;
using Core;

namespace Game.Http.Listener
{
	public class UCPListener : RequestScript
	{
		[Request("/createaccount")]
		public static void CreateAccount(int forumId, string name)
		{
			if (AccountService.CheckForumId(forumId)) return;

			var pos = new PositionModel(-1042.4572f, -2745.323f, 21.343628f, 0, 0, -0.49473903f);
			PositionService.Add(pos);

			var custom = new CustomizationModel();
			CustomizationService.Add(custom);

			var clothes = new ClothesModel();
			ClothesService.Add(clothes);

			var inv = new InventoryModel(Config.Inventory.DEFAULT_SLOTS, Config.Inventory.DEFAULT_WEIGHT, InventoryType.PLAYER);
			var labInput = new InventoryModel(8, 30f, InventoryType.LAB_INPUT);
			var labOutput = new InventoryModel(8, 60f, InventoryType.LAB_OUTPUT);
			var locker = new InventoryModel(8, 100f, InventoryType.LOCKER);
			InventoryService.Add(inv, labInput, labOutput, locker);

			var license = new LicenseModel();
			LicenseService.Add(license);

			var account = new AccountModel(
				name,
				string.Empty,
				0,
				0,
				0,
				forumId,
				0,
				5000,
				40000,
				AccountService.GenerateUniquePhoneNumber(),
				pos.Id,
				custom.Id,
				clothes.Id,
				inv.Id,
				labInput.Id,
				labOutput.Id,
				locker.Id,
				0,
				license.Id,
				false);
			AccountService.Add(account);
		}
	}
}
