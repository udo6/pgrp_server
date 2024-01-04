using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models.Dealer
{
	public class DealerCache
	{
		public int DealerId { get; set; }
		public List<DealerItemCache> Items { get; set; }

		public DealerCache()
		{
			Items = new();
		}

		public DealerCache(int dealerId)
		{
			DealerId = dealerId;
			Items = new();
		}

		public DealerCache(int dealerId, List<DealerItemCache> items) : this(dealerId)
		{
			Items = items;
		}
	}
}
