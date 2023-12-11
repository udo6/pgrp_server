using AltV.Net;
using AltV.Net.Data;
using Core.Entities;
using Core.Enums;
using Core.Extensions;
using Database.Models.House;
using Database.Services;

namespace Game.Controllers
{
    public static class HouseController
	{
		public static readonly Dictionary<HouseType, int> HousePositions = new()
		{
			{ HouseType.FLAT, 449 },
			{ HouseType.LOW, 450 },
			{ HouseType.MID, 451 },
			{ HouseType.HIGH, 452 },
			{ HouseType.VILLA, 453 }
		};

		public static readonly Dictionary<HouseType, int> HouseWardrobePositions = new()
		{
			{ HouseType.FLAT, 461 },
			{ HouseType.LOW, 462 },
			{ HouseType.MID, 463 },
			{ HouseType.HIGH, 464 },
			{ HouseType.VILLA, 465 }
		};

		public static readonly Dictionary<HouseType, Position> HouseInventoryPositions = new()
		{
			{ HouseType.FLAT, new(265.80658f, -999.3494f, -99.01465f) },
			{ HouseType.LOW, new(351.2835f, -998.9934f, -99.19995f) },
			{ HouseType.MID, new(351.2835f, -998.9934f, -99.19995f) },
			{ HouseType.HIGH, new(-765.61316f, 327.3231f, 211.3927f) },
			{ HouseType.VILLA, new(-772.53625f, 326.51868f, 223.25488f) }
		};

		public static readonly Dictionary<HouseType, int> HousePrices = new()
		{
			{ HouseType.FLAT, 50000 },
			{ HouseType.LOW, 200000 },
			{ HouseType.MID, 500000 },
			{ HouseType.HIGH, 1000000 },
			{ HouseType.VILLA, 2500000 }
		};

		public static void LoadHouse(HouseModel model)
		{
			var pos = PositionService.Get(model.PositionId);
			if (pos == null) return;

			if (model.OwnerId < 1)
			{
				var shape = (RPShape)Alt.CreateColShapeCylinder(pos.Position.Down(), 2f, 2f);
				shape.Id = model.Id;
				shape.ShapeType = ColshapeType.HOUSE;
				shape.Size = 2f;
			}

			var inv = (RPShape)Alt.CreateColShapeCylinder(HouseInventoryPositions[model.Type].Down(), 2f, 2f);
			inv.Id = model.Id;
			inv.ShapeType = ColshapeType.HOUSE_INVENTORY;
			inv.Size = 2f;
			inv.Dimension = model.Id;
			inv.InventoryId = model.InventoryId;
			inv.InventoryAccess = new() { (model.OwnerId, OwnerType.PLAYER) };
		}
	}
}