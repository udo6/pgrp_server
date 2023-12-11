namespace Database.Models.Workstation
{
    public class WorkstationItemModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int WorkstationId { get; set; }
        public int ItemId { get; set; }
        public int ItemAmount { get; set; }
        public DateTime Added { get; set; }
        public int TimeLeft { get; set; }

        public WorkstationItemModel()
        {

        }

        public WorkstationItemModel(int accountId, int workstationId, int itemId, int itemAmount, DateTime added, int timeLeft)
        {
            AccountId = accountId;
            WorkstationId = workstationId;
            ItemId = itemId;
            ItemAmount = itemAmount;
            Added = added;
            TimeLeft = timeLeft;
        }
    }
}