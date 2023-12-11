namespace Logs.Models
{
	public class PlayerConnectionModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public bool Join { get; set; }
		public DateTime DateTime { get; set; }

		public PlayerConnectionModel()
		{
		}

		public PlayerConnectionModel(int accountId, bool join)
		{
			AccountId = accountId;
			Join = join;
			DateTime = DateTime.Now;
		}
	}
}