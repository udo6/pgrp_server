namespace Database.Models.Hospital
{
	public class HospitalBedModel
	{
		public int Id { get; set; }
		public int HospitalId { get; set; }
		public int PositionId { get; set; }

		public HospitalBedModel()
		{
		}

		public HospitalBedModel(int hospitalId, int positionId)
		{
			HospitalId = hospitalId;
			PositionId = positionId;
		}
	}
}