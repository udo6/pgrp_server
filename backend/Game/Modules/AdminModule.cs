using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Models.Account;
using Database.Services;
using Game.Controllers;

namespace Game.Modules
{
    public static class AdminModule
	{
		private static List<ClothesModel> MaleAdminClothes = new()
		{
			new(), // SPIELER
			new(135, 9, 0, 287, 9, 0, 10, 0, 0, 15, 0, 0, 114, 9, 0, 78, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // GUIDE
			new(135, 5, 0, 287, 5, 0, 10, 0, 0, 15, 0, 0, 114, 5, 0, 78, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // SUPPORTER
			new(135, 4, 0, 287, 4, 0, 10, 0, 0, 15, 0, 0, 114, 4, 0, 78, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // MODERATOR
			new(135, 3, 0, 287, 3, 0, 10, 0, 0, 15, 0, 0, 114, 3, 0, 78, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // ADMINISTRATOR
			new(135, 12, 0, 287, 12, 0, 10, 0, 0, 15, 0, 0, 114, 12, 0, 78, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // SUPERADMIN
			new(135, 2, 0, 287, 2, 0, 10, 0, 0, 15, 0, 0, 114, 2, 0, 78, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // MANAGER
			new(135, 2, 0, 287, 2, 0, 10, 0, 0, 15, 0, 0, 114, 2, 0, 78, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // PL
		};

		private static List<ClothesModel> FemaleAdminClothes = new()
		{
			new(), // SPIELER
			new(135, 9, 0, 300, 9, 0, 10, 0, 0, 15, 0, 0, 121, 9, 0, 82, 9, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // GUIDE
			new(135, 5, 0, 300, 5, 0, 10, 0, 0, 15, 0, 0, 121, 5, 0, 82, 5, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // SUPPORTER
			new(135, 4, 0, 300, 4, 0, 10, 0, 0, 15, 0, 0, 121, 4, 0, 82, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // MODERATOR
			new(135, 3, 0, 300, 3, 0, 10, 0, 0, 15, 0, 0, 121, 3, 0, 82, 3, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // ADMINISTRATOR
			new(135, 12, 0, 300, 12, 0, 10, 0, 0, 15, 0, 0, 121, 12, 0, 82, 12, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // SUPERADMIN
			new(135, 2, 0, 300, 2, 0, 10, 0, 0, 15, 0, 0, 121, 2, 0, 82, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // MANAGER
			new(135, 2, 0, 300, 2, 0, 10, 0, 0, 15, 0, 0, 121, 2, 0, 82, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0, -1, 0, 0), // PL
		};

		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer>("Server:Admin:ToggleDuty", ToggleDuty);
			Alt.OnClient<RPPlayer, bool>("Server:Admin:ToggleNoclip", ToggleNoclip);
		}

		private static void ToggleDuty(RPPlayer player)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPPORTER) return;

			var custom = CustomizationService.Get(player.CustomizationId);
			if (custom == null) return;

			player.AdminDuty = !player.AdminDuty;

			player.Invincible = player.AdminDuty;
			player.Emit("Client:AdminModule:SetDuty", player.AdminDuty);

			if (player.AdminDuty)
			{
				PlayerController.ApplyPlayerClothes(player, (custom.Gender ? MaleAdminClothes : FemaleAdminClothes)[(int)player.AdminRank]);
			}
			else
			{
				PlayerController.ApplyPlayerClothes(player);
				player.Streamed = true;
				player.Visible = true;
			}
		}

		private static void ToggleNoclip(RPPlayer player, bool state)
		{
			if (player.AdminRank < Core.Enums.AdminRank.MODERATOR) return;

			player.Streamed = !state;
			player.Visible = player.Streamed;
		}
	}
}