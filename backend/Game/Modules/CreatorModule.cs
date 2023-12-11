using AltV.Net;
using Core.Attribute;
using Core.Entities;
using Database.Models.Account;
using Database.Services;
using Game.Controllers;
using Newtonsoft.Json;

namespace Game.Modules
{
    public static class CreatorModule
	{
		[Initialize]
		public static void Initialize()
		{
			Alt.OnClient<RPPlayer, string>("Server:Creator:Finish", Finish);
		}

		private static void Finish(RPPlayer player, string data)
		{
			var custom = JsonConvert.DeserializeObject<CustomizationModel>(data);
			if (custom == null) return;

			if(custom.Id != player.CustomizationId) custom.Id = player.CustomizationId;

			custom.Finished = true;

			CustomizationService.Update(custom);

			PlayerController.LoadPlayer(player);

			player.ShowComponent("Creator", false);
		}
	}
}