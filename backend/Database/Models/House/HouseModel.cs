using Core.Enums;

namespace Database.Models.House
{
    public class HouseModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public HouseType Type { get; set; }
        public int OwnerId { get; set; }
        public int InventoryId { get; set; }
        public int KeyHolderId { get; set; }
        public int JumppointId { get; set; }
        public int WardrobeId { get; set; }

        public HouseModel()
        {
        }

        public HouseModel(int positionId, HouseType type, int ownerId, int inventoryId, int keyHolderId, int jumppointId, int wardrobeId)
        {
            PositionId = positionId;
            Type = type;
            OwnerId = ownerId;
            InventoryId = inventoryId;
            KeyHolderId = keyHolderId;
            JumppointId = jumppointId;
            WardrobeId = wardrobeId;
        }
    }
}