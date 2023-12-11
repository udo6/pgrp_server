namespace Database.Models.Lootdrop
{
    public class LootdropLootModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public float Probability { get; set; }

        public LootdropLootModel()
        {
        }

        public LootdropLootModel(int itemId, float probability)
        {
            ItemId = itemId;
            Probability = probability;
        }
    }
}