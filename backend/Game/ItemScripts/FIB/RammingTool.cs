using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Database.Services;

namespace Game.ItemScripts.FIB
{
    public class RammingTool : ItemScript
	{
		public RammingTool() : base(288, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.TeamId != 2 || !player.TeamDuty) return;

			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.JUMP_POINT);
			if (shape == null) return;

			var jumppoint = JumppointService.Get(shape.ShapeId);
			if (jumppoint == null || jumppoint.Type != JumppointType.HOUSE) return;

			player.PlayAnimation(AnimationType.RAMMING);
			player.StartInteraction(() =>
			{
				if (player == null || !player.Exists) return;

				jumppoint.Locked = false;
				jumppoint.LastCrack = DateTime.Now;
				JumppointService.Update(jumppoint);

				player.Notify("Information", "Du hast das Haus geöffnet!", NotificationType.SUCCESS);
			}, 90000);
		}
	}
}