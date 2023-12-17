using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Models.Dealer;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Gamedesign
{
	public static class DealerCommands
	{
		[Command("createdealer")]
		public static void CreateDealer(RPPlayer player)
		{
			if (!Core.Config.DevMode && player.AdminRank < Core.Enums.AdminRank.SUPERADMIN) return;

			var pos = new PositionModel(player.Position, player.Rotation);
			PositionService.Add(pos);

			var dealer = new DealerModel(pos.Id);
			DealerService.Add(dealer);
			DealerController.LoadDealer(dealer, true);
			player.Notify("Gamedesign", "Du hast einen Dealer erstellt!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
