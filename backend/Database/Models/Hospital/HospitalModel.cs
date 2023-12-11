namespace Database.Models.Hospital
{
	public class HospitalModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }

		public HospitalModel()
		{
		}

		public HospitalModel(int positionId)
		{
			PositionId = positionId;
		}
	}
}