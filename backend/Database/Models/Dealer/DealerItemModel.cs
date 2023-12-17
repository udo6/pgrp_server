namespace Database.Models.Dealer
{
	public class DealerItemModel
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		public int Price { get; set; }
		public int MinPrice { get; set; }
		public int MaxPrice { get; set; }

		public DealerItemModel()
		{
		}

		public DealerItemModel(int itemId, int price, int minPrice, int maxPrice)
		{
			ItemId = itemId;
			Price = price;
			MinPrice = minPrice;
			MaxPrice = maxPrice;
		}
	}
}
