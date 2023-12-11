using AltV.Net.Elements.Entities;
using Core.Attribute;
using Core.Entities;
using Core.Enums;

namespace Game.Modules
{
	public static class ColshapeModule
	{
		public static List<(ColshapeType, (string, string))> Interactions = new()
		{
			(ColshapeType.GANGWAR_START, ("KEY_E", "GANGWAR")),
			(ColshapeType.GANGWAR_SPAWN, ("KEY_E", "GANGWARSPAWN")),
			(ColshapeType.LABORATORY_INTERACTION, ("KEY_E", "LABORATORY")),
			(ColshapeType.TEAM, ("KEY_E", "TEAM")),
			(ColshapeType.HOUSE, ("KEY_E", "HOUSE")),
			(ColshapeType.WARDROBE, ("KEY_E", "WARDROBE")),
			(ColshapeType.WAREHOUSE, ("KEY_E", "WAREHOUSE")),
			(ColshapeType.WAREHOUSE_UPGRADE, ("KEY_E", "WAREHOUSE_UPGRADE")),
			(ColshapeType.SWAT_SHOP, ("KEY_E", "SWAT_SHOP")),
			(ColshapeType.VEHICLE_SHOP, ("KEY_E", "VEHICLE_SHOP")),
			(ColshapeType.LIFEINVADER, ("KEY_E", "LIFEINVADER")),
			(ColshapeType.FARMING_SPOT, ("KEY_E", "FARMING")),
			(ColshapeType.PROCESSOR, ("KEY_E", "PROCESSOR")),
			(ColshapeType.EXPORT_DEALER, ("KEY_E", "EXPORT_DEALER")),
			(ColshapeType.IMPOUND, ("KEY_E", "IMPOUND")),
			(ColshapeType.FFA, ("KEY_E", "FFA")),
			(ColshapeType.DROP_BOX, ("KEY_E", "LOOTDROP")),
			(ColshapeType.DEALER, ("KEY_E", "DEALER")),
			(ColshapeType.DMV, ("KEY_E", "DMV")),
			(ColshapeType.SOCIAL_BONUS, ("KEY_E", "SOCIAL_BONUS")),
			(ColshapeType.TUNER, ("KEY_E", "TUNER")),

			(ColshapeType.DOOR, ("KEY_L", "DOOR_LOCK")),

			(ColshapeType.JUMP_POINT, ("KEY_E", "JUMPPOINT")),
			(ColshapeType.JUMP_POINT, ("KEY_L", "JUMPPOINT")),
		};

		[ServerEvent(ServerEventType.ENTITY_COLSHAPE)]
		public static void OnEntityColshape(RPShape shape, IWorldObject entity, bool entered)
		{
			if (entity.Type != BaseObjectType.Player) return;

			var player = (RPPlayer)entity;

			player.Emit(entered ? "Client:PlayerModule:EnterColshape" : "Client:PlayerModule:ExitColshape", shape.Id, (int)shape.ShapeType);

			foreach(var item in Interactions.Where(x => x.Item1 == shape.ShapeType))
			{
				if (entered) player.SetInteraction(item.Item2.Item1, item.Item2.Item2);
				else player.SetInteraction(item.Item2.Item1, "");
			}
		}
	}
}