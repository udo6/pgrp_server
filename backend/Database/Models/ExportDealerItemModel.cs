namespace Database.Models
{
	public class ExportDealerItemModel
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		public int Price { get; set; }

		public ExportDealerItemModel()
		{
		}

		public ExportDealerItemModel(int itemId, int price)
		{
			ItemId = itemId;
			Price = price;
		}
	}
}