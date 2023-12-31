using Core.Attribute;
using Core.Entities;
using Game.Modules;

namespace Game.Commands.Admin
{
	public static class LifeinvaderCommands
	{
		[Command("deletelifeinvaderpost")]
		public static void DeleteLifeinvaderPost(RPPlayer player, int postId)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPPORTER) return;

			var post = LifeinvaderModule.Posts.FirstOrDefault(x => x.Id == postId);
			if (post == null)
			{
				player.Notify("Administration", "Es konnte kein Post mit der ID gefunden werden!", Core.Enums.NotificationType.ERROR);
				return;
			}

			LifeinvaderModule.Posts.Remove(post);
			player.Notify("Administration", "Du hast einen Lifeinvader Post gelöscht!", Core.Enums.NotificationType.SUCCESS);
		}

		[Command("clearlifeinvader")]
		public static void ClearLifeinvader(RPPlayer player)
		{
			if (player.AdminRank < Core.Enums.AdminRank.ADMINISTRATOR) return;

			LifeinvaderModule.Posts.Clear();
			player.Notify("Administration", "Du hast alle Lifeinvader Posts gelöscht!", Core.Enums.NotificationType.SUCCESS);
		}
	}
}
