using Core.Entities;
using Core.Enums;
using Database.Models.Inventory;
using Newtonsoft.Json;

namespace Game.ItemScripts
{
	public class NumberPlate : ItemScript
	{
		public NumberPlate() : base(373, true)
		{
		}

		public override void OnUse(RPPlayer player, InventoryModel inventory, InventoryItemModel item, int slot, int amount)
		{
			if (player.TeamId != 4 || !player.TeamDuty || !player.IsInVehicle) return;

			player.ShowComponent("Input", true, JsonConvert.SerializeObject(new
			{
				Title = "KFZ Anmeldung",
				Message = "Gebe das Kennzeichen an",
				Type = (int)InputType.TEXT,
				CallbackEvent = "Server:Impound:SetPlate",
				CallbackArgs = new List<object>()
			}));
		}
	}
}
