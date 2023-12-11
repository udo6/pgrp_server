using Core.Enums;

namespace Database.Models.Bank
{
    public class BankModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public BankType Type { get; set; }

        public BankModel()
        {
            Name = string.Empty;
        }

        public BankModel(string name, int positionId, BankType type)
        {
            Name = name;
            PositionId = positionId;
            Type = type;
        }
    }
}