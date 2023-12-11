using Logs.Enums;

namespace Logs.Models
{
	public class MoneyTransactionModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int TargetId { get; set; }
		public int Amount { get; set; }
		public MoneyTransactionType Type { get; set; }
		public DateTime DateTime { get; set; }

		public MoneyTransactionModel()
		{
		}

		public MoneyTransactionModel(int accountId, int targetId, int amount, MoneyTransactionType type)
		{
			AccountId = accountId;
			TargetId = targetId;
			Amount = amount;
			Type = type;
			DateTime = DateTime.Now;
		}
	}
}