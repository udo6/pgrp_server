namespace Core.Models.Dealer
{
	public class DealerItemCache
	{
		public int Id { get; set; }
		public int Price { get; set; }
		public int SellCap { get; set; }
		public int ItemsSold { get; set; }

		public DealerItemCache()
		{
		}

		public DealerItemCache(int id, int price, int sellCap)
        {
            Id = id;
            Price = price;
            SellCap = sellCap;
            ItemsSold = 0;
        }
    }
}
