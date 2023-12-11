namespace Database.Models.DPOS
{
	public class ImpoundModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }

		public ImpoundModel()
		{
		}

		public ImpoundModel(int positionId)
		{
			PositionId = positionId;
		}
	}
}