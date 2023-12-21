namespace Database.Models.Case
{
	public class CaseLootModel
	{
		public int Id { get; set; }
		public int CaseId { get; set; }
		public int ItemId { get; set; }
		public int ItemAmount { get; set; }
		public float Probability { get; set; }

		public CaseLootModel()
		{
		}

		public CaseLootModel(int caseId, int itemId, int itemAmount, float probability)
		{
			CaseId = caseId;
			ItemId = itemId;
			ItemAmount = itemAmount;
			Probability = probability;
		}
	}
}
