namespace Database.Models.Dealer
{
	public class DealerModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public bool Active { get; set; }

		public DealerModel()
		{
		}

		public DealerModel(int positionId, bool active)
		{
			PositionId = positionId;
			Active = active;
		}
	}
}
