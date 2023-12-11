namespace Database.Models.Barber
{
	public class BarberModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }

		public BarberModel()
		{
		}

		public BarberModel(int positionId)
		{
			PositionId = positionId;
		}
	}
}