using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
using Newtonsoft.Json;

namespace Game.Commands.Player
{
	public static class WarehouseCommands
	{
		[Command("sellwarehouse")]
		public static void SellWarehouse(RPPlayer player)
		{
			if (!player.LoggedIn || !WarehouseService.HasWarehouse(player.DbId, OwnerType.PLAYER)) return;

			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Lagerhalle",
				Message = "Bist du dir sicher, dass du deine Lagerhalle für 3/4 des Kaufpreises verkaufen möchtest?",
				Type = (int)InputType.CONFIRM,
				CallbackEvent = "Server:Warehouse:Sell",
				CallbackArgs = new List<object>()
			}));
		}
	}
}
