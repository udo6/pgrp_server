using AltV.Net;
using AltV.Net.Data;
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

		public static void SendToCreator(RPPlayer player, CustomizationModel customization, Position outsidePos)
		{
			var pos = PositionService.Get(player.PositionId);
			if(pos != null)
			{
				pos.Position = outsidePos;
				PositionService.Update(pos);
			}

			player.InInterior = true;
			player.OutsideInteriorPosition = outsidePos;
			player.SetPosition(new(402.8664f, -996.4108f, -100f));
			player.Visible = false;
			player.ShowComponent("Creator", true, JsonConvert.SerializeObject(customization));
		}

		private static void Finish(RPPlayer player, string data)
		{
			var custom = JsonConvert.DeserializeObject<CustomizationModel>(data);
			if (custom == null) return;

			if(custom.Id != player.CustomizationId) custom.Id = player.CustomizationId;

			custom.Finished = true;
			CustomizationService.Update(custom);

			player.Visible = true;
			PlayerController.LoadPlayer(player);

			player.ShowComponent("Creator", false);
		}
	}
}