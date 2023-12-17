using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Admin
{
	public static class DealerCommands
	{
		[Command("activatedealer")]
		public static void ActivateDealer(RPPlayer player, int id)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var dealer = DealerService.Get(id);
			if (dealer == null) return;

			DealerController.LoadDealer(dealer, true);
			player.Notify("Administration", "Du hast einen Dealer geladen!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
