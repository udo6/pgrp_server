using Core.Enums;

namespace Database.Models.Shop
{
    public class ShopModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public int PedPositionId { get; set; }
        public ShopType Type { get; set; }
        public int OwnerId { get; set; }

        public ShopModel()
        {
            Name = string.Empty;
        }

        public ShopModel(string name, int positionId, int pedPosId, ShopType type, int ownerId)
        {
            Name = name;
            PositionId = positionId;
            PedPositionId = pedPosId;
            Type = type;
            OwnerId = ownerId;
        }
    }
}