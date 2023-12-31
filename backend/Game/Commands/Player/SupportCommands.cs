using Core.Attribute;
using Core.Entities;
using Database.Models;
using Database.Services;
using Game.Controllers;
using Game.Modules;

namespace Game.Commands.Player
{
	public static class SupportCommands
	{
		[Command("support", true)]
		public static void CreateTicket(RPPlayer player, string message)
		{
			var existingTicket = SupportModule.Tickets.FirstOrDefault(x => x.CreatorId == player.DbId);
			if (existingTicket != null)
			{
				if(message.ToLower() == "cancel")
				{
					SupportModule.Tickets.Remove(existingTicket);
					player.Notify("Support", "Du hast dein Ticket geschlossen!", Core.Enums.NotificationType.ERROR);
					return;
				}

				player.Notify("Support", "Du hast bereits ein Ticket! Nutze /support cancel um es zu schließen.", Core.Enums.NotificationType.ERROR);
				return;
			}

			SupportModule.Tickets.Add(new(player.DbId, player.Name, message));
			player.Notify("Ticket System", $"Dein Ticket wurde an die Administration weitergeleitet!", Core.Enums.NotificationType.INFO);
			AdminController.BroadcastTeam("Ticket System", $"Es ist ein Ticket von {player.Name} eingegangen! Nachricht: {message}", Core.Enums.NotificationType.INFO);
		}

		[Command("r")]
		public static void Report(RPPlayer player)
		{
			var model = new ReportModel(player.DbId, player.LastAttackerId, DateTime.Now);
			ReportService.Add(model);

			player.Notify("Report System", $"Dein Report wurde erstellt und an die Administration weitergeleitet! Report-ID: {model.Id}", Core.Enums.NotificationType.INFO);
		}
	}
}
