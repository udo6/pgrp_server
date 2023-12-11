namespace Database.Models.Shop
{
    public class ShopItemModel
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int ItemId { get; set; }
        public int Price { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinRank { get; set; }

        public ShopItemModel()
        {
        }

        public ShopItemModel(int shopId, int itemId, int price, int minPrice, int maxPrice, int minRank)
        {
            ShopId = shopId;
            ItemId = itemId;
            Price = price;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            MinRank = minRank;
        }
    }
}