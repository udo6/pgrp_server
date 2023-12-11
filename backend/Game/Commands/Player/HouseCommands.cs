using Core.Attribute;
using Core.Entities;
using Core.Enums;
using Newtonsoft.Json;

namespace Game.Commands.Player
{
	public static class HouseCommands
	{
		[Command("sellhouse")]
		public static void SellHouse(RPPlayer player)
		{
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