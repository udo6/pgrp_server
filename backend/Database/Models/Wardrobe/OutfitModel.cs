namespace Database.Models.Wardrobe
{
    public class OutfitModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }
        public int ClothesId { get; set; }

        public OutfitModel()
        {
            Name = string.Empty;
        }

        public OutfitModel(string name, int accountId, int clothesId)
        {
            Name = name;
            AccountId = accountId;
            ClothesId = clothesId;
        }
    }
}