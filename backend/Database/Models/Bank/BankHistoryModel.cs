using Core.Enums;

namespace Database.Models.Bank
{
    public class BankHistoryModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public TransactionType Type { get; set; }
        public bool Withdraw { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public BankHistoryModel()
        {
            Name = string.Empty;
            BankName = string.Empty;
        }

        public BankHistoryModel(int accountId, string name, string bankName, TransactionType type, bool withdraw, int amount, DateTime date)
        {
            AccountId = accountId;
            Name = name;
            BankName = bankName;
            Type = type;
            Withdraw = withdraw;
            Amount = amount;
            Date = date;
        }
    }
}