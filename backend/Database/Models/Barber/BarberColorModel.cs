namespace Database.Models.Barber
{
	public class BarberColorModel
	{
		public int Id { get; set; }
		public string HexColor { get; set; }
		public int Value { get; set; }
		public int Price { get; set; }

		public BarberColorModel()
		{
			HexColor = string.Empty;
		}

		public BarberColorModel(string hexColor, int value, int price)
		{
			HexColor = hexColor;
			Value = value;
			Price = price;
		}
	}
}