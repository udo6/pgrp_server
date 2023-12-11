namespace Database.Models.Tattoo
{
	public class TattooShopModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }

		public TattooShopModel()
		{
		}

		public TattooShopModel(int positionId)
		{
			PositionId = positionId;
		}
	}
}