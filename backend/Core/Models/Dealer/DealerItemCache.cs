using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Dealer
{
	public class DealerItemCache
	{
		public int Id { get; set; }
		public int Price { get; set; }

		public DealerItemCache()
		{
		}

		public DealerItemCache(int id, int price)
		{
			Id = id;
			Price = price;
		}
	}
}
