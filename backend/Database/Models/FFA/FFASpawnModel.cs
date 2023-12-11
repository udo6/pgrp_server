namespace Database.Models.FFA
{
	public class FFASpawnModel
	{
		public int Id { get; set; }
		public int FFAId { get; set; }
		public int PositionId { get; set; }

		public FFASpawnModel()
		{
		}

		public FFASpawnModel(int fFAId, int positionId)
		{
			FFAId = fFAId;
			PositionId = positionId;
		}
	}
}