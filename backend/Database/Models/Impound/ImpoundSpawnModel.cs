namespace Database.Models.DPOS
{
	public class ImpoundSpawnModel
	{
		public int Id { get; set; }
		public int ImpoundId { get; set; }
		public int PositionId { get; set; }

		public ImpoundSpawnModel()
		{
		}

		public ImpoundSpawnModel(int impoundId, int positionId)
		{
			ImpoundId = impoundId;
			PositionId = positionId;
		}
	}
}