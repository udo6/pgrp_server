namespace Database.Models.Dealer
{
	public class DealerModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }

		public DealerModel()
		{
		}

		public DealerModel(int positionId)
		{
			PositionId = positionId;
		}
	}
}
