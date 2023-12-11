﻿using Core.Attribute;
using Core.Entities;
using Database.Services;
using Game.Controllers;

namespace Game.Commands.Admin
{
	public static class InventoryCommands
	{
		[Command("giveitem")]
		public static void GiveItem(RPPlayer player, int itemId, int amount)
		{
			if (player.AdminRank < Core.Enums.AdminRank.SUPERADMINISTRATOR) return;

			var inventory = InventoryService.Get(player.InventoryId);
			if (inventory == null) return;

			var item = InventoryService.GetItem(itemId);
			if (item == null) return;

			InventoryController.AddItem(inventory, item, amount);
		}
	}
}