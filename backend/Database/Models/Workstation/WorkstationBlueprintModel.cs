namespace Database.Models.Workstation
{
    public class WorkstationBlueprintModel
    {
        public int Id { get; set; }
        public int WorkstationId { get; set; }
        public int ItemId { get; set; }
        public int ItemAmount { get; set; }
        public int Price { get; set; }
        public int NeededItem { get; set; }
        public int NeededItemAmount { get; set; }
        public int Duration { get; set; }
        public int Max { get; set; }
        public bool Active { get; set; }

        public WorkstationBlueprintModel()
        {

        }

        public WorkstationBlueprintModel(int stationId, int itemId, int itemAmount, int price, int neededItem, int neededItemAmount, int duration, int max, bool active)
        {
            WorkstationId = stationId;
            ItemId = itemId;
            ItemAmount = itemAmount;
            Price = price;
            NeededItem = neededItem;
            NeededItemAmount = neededItemAmount;
            Duration = duration;
            Max = max;
            Active = active;
        }
    }
}