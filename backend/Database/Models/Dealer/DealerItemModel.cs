namespace Database.Models.Dealer
{
	public class DealerItemModel
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		public int Price { get; set; }

		public DealerItemModel()
		{
		}

		public DealerItemModel(int itemId, int price)
		{
			ItemId = itemId;
			Price = price;
		}
	}
}
