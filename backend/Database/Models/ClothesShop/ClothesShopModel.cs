using Core.Enums;

namespace Database.Models.ClothesShop
{
    public class ClothesShopModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public ClothesShopType Type { get; set; }

        public ClothesShopModel()
        {
            Name = string.Empty;
        }

        public ClothesShopModel(string name, int positionId, ClothesShopType type)
        {
            Name = name;
            PositionId = positionId;
            Type = type;
        }
    }
}