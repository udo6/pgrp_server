using AltV.Net;
using AltV.Net.Data;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models;
using Database.Models.Warehouse;
using Database.Services;

namespace Game.Controllers
{
    public static class WarehouseController
	{
		public static readonly int WarehouseBoxSlots = 16;
		public static readonly float WarehouseBoxWeight = 200f;

		public static readonly int WarehouseBuyPrice = 100000;
		public static readonly int WarehouseUpgradePrice = 150000;
		public static readonly int WarehouseUpgradeBoxPrice = 10000;

		public static readonly Dictionary<WarehouseType, Position> UpgraderPositions = new()
		{
			{ WarehouseType.SMALL, new(1088.3605f, -3101.3274f, -39.01245f) },
			{ WarehouseType.MEDIUM, new(1049.0374f, -3100.6946f, -39.01245f) },
			{ WarehouseType.LARGE, new(994.5099f, -3100.233f, -39.01245f) }
		};

		public static readonly int SmallExitPositionId = 456;
		public static readonly List<PositionModel> SmallBoxPositions = new() // 6
		{
			new(1088.7164f, -3096.7780f, -39.51245f, 0, 0, 0),
			new(1091.3011f, -3096.7780f, -39.51245f, 0, 0, 0),
			new(1095.0198f, -3096.7780f, -39.51245f, 0, 0, 0),
			new(1097.6307f, -3096.7780f, -39.51245f, 0, 0, 0),
			new(1101.2836f, -3096.7780f, -39.51245f, 0, 0, 0),
			new(1103.8682f, -3096.7780f, -39.51245f, 0, 0, 0)
		};

		public static readonly int MediumExitPositionId = 455;
		public static readonly List<PositionModel> MediumBoxPositions = new() // 21
		{
			new(1067.6307f, -3109.5298f, -39.51245f, 0, 0, 0),
			new(1065.2043f, -3109.5298f, -39.51245f, 0, 0, 0),
			new(1062.8044f, -3109.5298f, -39.51245f, 0, 0, 0),
			new(1060.3649f, -3109.5298f, -39.51245f, 0, 0, 0),
			new(1057.9385f, -3109.5298f, -39.51245f, 0, 0, 0),
			new(1055.5516f, -3109.5298f, -39.51245f, 0, 0, 0),
			new(1053.1516f, -3109.5298f, -39.51245f, 0, 0, 0),

			new(1053.0593f, -3102.6462f, -39.51245f, 0, 0, 0),
			new(1055.4330f, -3102.6462f, -39.51245f, 0, 0, 0),
			new(1057.8989f, -3102.6462f, -39.51245f, 0, 0, 0),
			new(1060.1274f, -3102.6462f, -39.51245f, 0, 0, 0),
			new(1062.6329f, -3102.6462f, -39.51245f, 0, 0, 0),
			new(1064.9670f, -3102.6462f, -39.51245f, 0, 0, 0),
			new(1067.4462f, -3102.6462f, -39.51245f, 0, 0, 0),

			new(1067.4594f, -3095.7363f, -39.51245f, 0, 0, 0),
			new(1065.1120f, -3095.7363f, -39.51245f, 0, 0, 0),
			new(1062.6989f, -3095.7363f, -39.51245f, 0, 0, 0),
			new(1060.2594f, -3095.7363f, -39.51245f, 0, 0, 0),
			new(1057.8857f, -3095.7363f, -39.51245f, 0, 0, 0),
			new(1055.3802f, -3095.7363f, -39.51245f, 0, 0, 0),
			new(1053.0066f, -3095.7363f, -39.51245f, 0, 0, 0)
		};

		public static readonly int LargeExitPositionId = 454;
		public static readonly List<PositionModel> LargeBoxPositions = new() // 37
		{
			new(1026.4099f, -3106.5364f, -39.51245f, 0, 0, 0f),
			new (1026.4099f, -3108.9758f, -39.51245f, 0, 0, -1.5336909f),
			new(1026.4099f, -3111.1912f, -39.51245f, 0, 0, 1.5336909f),

			new(1018.18024f, -3108.5276f, -39.51245f, 0, 0, 1.5336909f),
			new(1015.7934f, -3108.5276f, -39.51245f, 0, 0, 0f),
			new(1013.24835f, -3108.5276f, -39.51245f, 0, 0, 0f),
			new(1010.96704f, -3108.5276f, -39.51245f, 0, 0, 1.5336909f),
			new(1008.60657f, -3108.5276f, -39.51245f, 0, 0, 0f),
			new(1006.03516f, -3108.5276f, -39.51245f, 0, 0, 0f),
			new(1003.45056f, -3108.5276f, -39.51245f, 0, 0, 1.5336909f),

			new(993.3143f, -3111.3098f, -39.51245f, 0, 0, -1.5336909f),
			new(993.3143f, -3109.0022f, -39.51245f, 0, 0, 1.5336909f),
			new(993.3143f, -3106.5364f, -39.51245f, 0, 0, 0f),

			new(1003.54285f, -3102.923f, -39.51245f, 0, 0, 0f),
			new(1005.87695f, -3102.923f, -39.51245f, 0, 0, 0f),
			new(1008.30330f, -3102.923f, -39.51245f, 0, 0, 1.5336909f),
			new(1010.91430f, -3102.923f, -39.51245f, 0, 0, 0f),
			new(1013.32745f, -3102.923f, -39.51245f, 0, 0, 0f),
			new(1015.81976f, -3102.923f, -39.51245f, 0, 0, 1.5336909f),
			new(1017.99560f, -3102.923f, -39.51245f, 0, 0, 0f),

			new(1018.12744f, -3097.0022f, -39.51245f, 0, 0, 0f),
			new(1015.55600f, -3097.0022f, -39.51245f, 0, 0, 1.5336909f),
			new(1013.14290f, -3097.0022f, -39.51245f, 0, 0, 0f),
			new(1010.82196f, -3097.0022f, -39.51245f, 0, 0, 0f),
			new(1008.63300f, -3097.0022f, -39.51245f, 0, 0, 1.5336909f),
			new(1005.99560f, -3097.0022f, -39.51245f, 0, 0, 0f),
			new(1003.84610f, -3097.0022f, -39.51245f, 0, 0, 0f),

			new(1003.56920f, -3092.0308f, -39.51245f, 0, 0, 1.5336909f),
			new(1005.75824f, -3092.0308f, -39.51245f, 0, 0, 0f),
			new(1008.25055f, -3092.0308f, -39.51245f, 0, 0, 0f),
			new(1010.75604f, -3092.0308f, -39.51245f, 0, 0, 1.5336909f),
			new(1013.24835f, -3092.0308f, -39.51245f, 0, 0, 0f),
			new(1015.52966f, -3092.0308f, -39.51245f, 0, 0, 0f),
			new(1017.99560f, -3092.0308f, -39.51245f, 0, 0, 1.5336909f),

			new(1026.4099f, -3091.6484f, -39.51245f, 0, 0, 1.5336909f),
			new(1026.4099f, -3093.8638f, -39.51245f, 0, 0, -1.5336909f),
			new(1026.4099f, -3096.29f, -39.51245f, 0, 0, 0f)
		};

		public static void LoadWarehouse(WarehouseModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			if (model.OwnerId < 1)
			{
				var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
				shape.Id = model.Id;
				shape.ShapeType = ColshapeType.WAREHOUSE;
				shape.Size = 2f;
			}

			var upgrader = (RPShape)Alt.CreateColShapeCylinder(UpgraderPositions[model.Type].Down(), 1.5f, 2f);
			upgrader.Id = model.Id;
			upgrader.ShapeType = ColshapeType.WAREHOUSE_UPGRADE;
			upgrader.Size = 1.5f;
			upgrader.Dimension = model.Id;

			var inventories = WarehouseService.GetFromWarehouse(model.Id);
			var positions = GetPositions(model.Type);
			for (var i = 0; i < inventories.Count; i++)
				LoadWarehouseInventory(model, inventories[i], positions[i]);
		}

		public static void LoadWarehouseInventory(WarehouseModel model, WarehouseInventoryModel inventory, PositionModel pos)
		{
			var inv = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 1.5f, 2f);
			inv.Id = model.Id;
			inv.ShapeType = ColshapeType.WAREHOUSE_BOX;
			inv.Size = 1.5f;
			inv.Dimension = model.Id;
			inv.InventoryId = inventory.InventoryId;
			inv.InventoryAccess = new()
			{
				(model.OwnerId, model.OwnerType),
				(model.KeyHolderId, OwnerType.PLAYER)
			};

			inv.Object = Alt.CreateObject(2107849419, pos.Position, pos.Rotation);
		}

		public static List<PositionModel> GetPositions(WarehouseType type)
		{
			switch (type)
			{
				case WarehouseType.SMALL: return SmallBoxPositions;
				case WarehouseType.MEDIUM: return MediumBoxPositions;
				case WarehouseType.LARGE: return LargeBoxPositions;
			}

			return new();
		}

		public static bool IsWarehouseOwner(RPPlayer player, int owner, OwnerType type)
		{
			return (type == OwnerType.PLAYER && player.DbId == owner) || (type == OwnerType.TEAM && player.TeamId == owner);
		}

		public static int GetExitPosition(WarehouseType type)
		{
			return type == WarehouseType.SMALL ? SmallExitPositionId : type == WarehouseType.MEDIUM ? MediumExitPositionId : LargeExitPositionId;
		}
	}
}