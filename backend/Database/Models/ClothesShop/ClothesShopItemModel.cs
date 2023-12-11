namespace Database.Models.ClothesShop
{
    public class ClothesShopItemModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int Component { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }
        public uint Dlc { get; set; }
        public bool IsProp { get; set; }
        public int ShopId { get; set; }
        public int Price { get; set; }
        public int Gender { get; set; }

        public ClothesShopItemModel()
        {
            Label = string.Empty;
        }

        public ClothesShopItemModel(string label, int component, int drawable, int texture, uint dlc, bool isProp, int shopId, int price, int gender)
        {
            Label = label;
            Component = component;
            Drawable = drawable;
            Texture = texture;
            Dlc = dlc;
            IsProp = isProp;
            ShopId = shopId;
            Price = price;
            Gender = gender;
        }
    }
}