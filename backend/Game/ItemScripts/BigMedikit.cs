using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Game.Controllers;

namespace Game.ItemScripts
{
	public class BigMedikit : ItemScript
	{
		public BigMedikit() : base(379, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (RPPlayer.All.Any(x => x.LoggedIn && x.TeamId == 3 && x.TeamDuty))
			{
				player.Notify("Information", "Du kannst dieses Item nur benutzen wenn keine LSMD Mitglieder online sind!", NotificationType.ERROR);
				return;
			}

			var target = RPPlayer.All.FirstOrDefault(x => x.LoggedIn && !x.Alive && !x.Coma && player.Position.Distance(x.Position) <= 5f);
			if (target == null)
			{
				player.Notify("Information", "Es ist keine Bewusstlose Person in deiner nähe!", NotificationType.ERROR);
				return;
			}

			player.PlayAnimation(AnimationType.PUT_IN_VEHICLE);
			player.StartInteraction(() =>
			{
				if (player == null || target == null || target.Alive) return;

				if(target.Coma)
				{
					player.Notify("Information", "Die Person scheint ins Koma gefallen zu sein!", NotificationType.ERROR);
					return;
				}

				PlayerController.SetPlayerAlive(target, false);
			}, 60000);
		}
	}
}
