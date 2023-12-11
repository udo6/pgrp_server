namespace Database.Models.Barber
{
	public class BarberStyleModel
	{
		public int Id { get; set; }
		public int BarberId { get; set; }
		public string Label { get; set; }
		public int Value { get; set; }
		public int Price { get; set; }
		public int Gender { get; set; }

		public BarberStyleModel()
		{
			Label = string.Empty;
		}

		public BarberStyleModel(int barberId, string label, int value, int price, int gender)
		{
			BarberId = barberId;
			Label = label;
			Value = value;
			Price = price;
			Gender = gender;
		}
	}
}