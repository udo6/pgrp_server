using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Database.Services;
using Newtonsoft.Json;

namespace Game.Commands.Player
{
	public static class HouseCommands
	{
		[Command("sellhouse")]
		public static void SellHouse(RPPlayer player)
		{
			if (!HouseService.HasPlayerHouse(player.DbId)) return;

			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "Haus",
				Message = "Bist du dir sicher, dass du dein Haus für 3/4 des Kaufpreises verkaufen möchtest?",
				Type = (int)InputType.CONFIRM,
				CallbackEvent = "Server:House:Sell",
				CallbackArgs = new List<object>()
			}));
		}
	}
}