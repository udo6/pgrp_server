using Core.Attribute;
using Core.Entities;
using Game.Modules.Scenario;

namespace Game.Commands.Admin
{
	public static class ScenarioCommands
	{
		[Command("spawnlootdrop")]
		public static void SpawnLootdrop(RPPlayer player)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var spawned = LootdropModule.Spawn();
			if (spawned) return;

			player.Notify("Administration", "Es konnte kein Drop gespawnt werden! (Kein Spot frei)", Core.Enums.NotificationType.ERROR);
		}
	}
}