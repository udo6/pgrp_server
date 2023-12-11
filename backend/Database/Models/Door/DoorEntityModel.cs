using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Door
{
	public class DoorEntityModel
	{
		public int Id { get; set; }
		public int DoorId { get; set; }
		public uint Model { get; set; }
		public int PositionId { get; set; }

		public DoorEntityModel()
		{
		}

		public DoorEntityModel(int doorId, uint model, int positionId)
		{
			DoorId = doorId;
			Model = model;
			PositionId = positionId;
		}
	}
}
