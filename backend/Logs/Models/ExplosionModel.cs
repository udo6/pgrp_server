using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Models
{
	public class ExplosionModel
	{
		public int Id { get; set; }
		public int PlayerId { get; set; }
		public int ExplosionType { get; set; }
		public DateTime Date { get; set; }

		public ExplosionModel()
		{
		}

		public ExplosionModel(int playerId, int explosionType, DateTime date)
		{
			PlayerId = playerId;
			ExplosionType = explosionType;
			Date = date;
		}
	}
}
