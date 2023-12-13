using Core.Entities;
using Core.Enums;
using Database.Services;
using Game.Modules.Scenario;
using Database.Models.Inventory;

namespace Game.ItemScripts
{
    public class WeldingRod : ItemScript
	{
		public WeldingRod() : base(25, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.Interaction) return;

			var shape = RPShape.Get(player.Position, player.Dimension, ColshapeType.JUMP_POINT);
			if (shape != null)
			{
				var jumppoint = JumppointService.Get(shape.Id);
				if (jumppoint == null || !IsJumppointCrackable(jumppoint.Type)) return;

				if (!jumppoint.Locked)
				{
					player.Notify("Information", "Der Jump Point ist bereits offen!", NotificationType.INFO);
					return;
				}

				if (jumppoint.Type == JumppointType.LABORATORY && RPPlayer.All.Where(x => x.TeamId == jumppoint.OwnerId).Count() < 15)
				{
					player.Notify("Information", "Es sind nicht genug Mitglieder der gegenpartei online!", NotificationType.ERROR);
					return;
				}

				NotifyOwner(jumppoint.Name, jumppoint.OwnerId, jumppoint.OwnerType);

				player.PlayAnimation(AnimationType.WELDING);
				player.StartInteraction(() =>
				{
					if (player == null || !player.Exists) return;

					jumppoint.Locked = false;
					jumppoint.LastCrack = DateTime.Now;
					JumppointService.Update(jumppoint);
					player.Notify("Information", "Du hast den Jump Point aufgebrochen!", NotificationType.SUCCESS);
				}, 90000);
			}

			var drop = LootdropModule.ActiveLootDrops.FirstOrDefault(x => !x.Open && x.Position.Distance(player.Position) <= 15f);
			if (drop != null)
			{
				player.PlayAnimation(AnimationType.WELDING);
				player.StartInteraction(() =>
				{
					if (drop.Open || player.Position.Distance(drop.Position) > 30f) return;

					LootdropModule.SpawnLoot(drop);
					player.Notify("Information", "Du konntest 3 Kisten aus dem Lootdrop entladen!", NotificationType.SUCCESS);
				}, 300000);
			}
		}

		private bool IsJumppointCrackable(JumppointType type)
		{
			return type == JumppointType.LABORATORY || type == JumppointType.FEDERAL;
		}

		private void NotifyOwner(string name, int owner, OwnerType type)
		{
			var owners = RPPlayer.All.Where(x => type == OwnerType.PLAYER && x.DbId == owner || type == OwnerType.TEAM && x.TeamId == owner);
			foreach(var player in owners)
			{
				player.Notify("Alarm", $"Es wurde ein Alarm bei folgendem Punkt gemeldet: {name}", NotificationType.WARN);
			}
		}
	}
}