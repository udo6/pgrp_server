using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Door
{
	public class DoorAccessModel
	{
		public int Id { get; set; }
		public int DoorId { get; set; }
		public int OwnerId { get; set; }
		public OwnerType OwnerType { get; set; }

		public DoorAccessModel()
		{
		}

		public DoorAccessModel(int doorId, int ownerId, OwnerType ownerType)
		{
			Id = doorId;
			OwnerId = ownerId;
			OwnerType = ownerType;
		}
	}
}
