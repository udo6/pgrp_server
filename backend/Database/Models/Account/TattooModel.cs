namespace Database.Models.Account
{
	public class TattooModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public uint Collection { get; set; }
		public uint Overlay { get; set; }

		public TattooModel()
		{
		}

		public TattooModel(int accountId, uint collection, uint overlay)
		{
			AccountId = accountId;
			Collection = collection;
			Overlay = overlay;
		}
	}
}