using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Tuner
{
	public class TunerModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }

		public TunerModel()
		{
		}

		public TunerModel(int positionId)
		{
			PositionId = positionId;
		}
	}
}
