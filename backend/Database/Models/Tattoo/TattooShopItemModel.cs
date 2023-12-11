namespace Database.Models.Tattoo
{
	public class TattooShopItemModel
	{
		public int Id { get; set; }
		public int TattooShopId { get; set; }
		public uint Collection { get; set; }
		public uint Overlay { get; set; }
		public int Category { get; set; }
		public string Label { get; set; }
		public int Price { get; set; }

		public TattooShopItemModel()
		{
			Label = string.Empty;
		}

		public TattooShopItemModel(int tattooShopId, uint collection, uint overlay, int category, string label, int price)
		{
			TattooShopId = tattooShopId;
			Collection = collection;
			Overlay = overlay;
			Category = category;
			Label = label;
			Price = price;
		}
	}
}