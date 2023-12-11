using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Door
{
	public class DoorModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public bool Locked { get; set; }
		public float Radius { get; set; }

		public DoorModel()
		{
		}

		public DoorModel(int positionId, bool locked, float radius)
		{
			PositionId = positionId;
			Locked = locked;
			Radius = radius;
		}
	}
}
