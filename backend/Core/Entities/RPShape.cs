using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using Core.Enums;

namespace Core.Entities
{
	public class RPShape : ColShape
	{
		public static readonly List<RPShape> All = new();
		public static RPShape? Get(Position pos, int dimension, ColshapeType type)
		{
			return All.FirstOrDefault(x => x.ShapeType == type && x.Dimension == dimension && new Position(x.Position.X, x.Position.Y, x.Position.Z+1).Distance(pos) <= x.Size);
		}

		public int Id { get; set; }
		public ColshapeType ShapeType { get; set; }
		public float Size { get; set; }

		public int InventoryId { get; set; }
		public List<(int Id, OwnerType Type)> InventoryAccess { get; set; }
		public bool InventoryLocked { get; set; }

		public bool JumppointEnterType { get; set; }

		public IObject? Object { get; set; }

		public RPShape(ICore core, nint nativePointer, uint id) : base(core, nativePointer, id)
		{
			All.Add(this);
			InventoryAccess = new();
		}

		public void Remove2()
		{
			All.Remove(this);
			Destroy();
		}
	}
}