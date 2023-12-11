using Core.Enums;

namespace Database.Models.Warehouse
{
    public class WarehouseModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public WarehouseType Type { get; set; }
        public int OwnerId { get; set; }
        public OwnerType OwnerType { get; set; }
        public int KeyHolderId { get; set; }
        public int JumppointId { get; set; }

        public WarehouseModel()
        {
        }

        public WarehouseModel(int positionId, WarehouseType type, int ownerId, OwnerType ownerType, int keyHolderId, int jumppointId)
        {
            PositionId = positionId;
            Type = type;
            OwnerId = ownerId;
            OwnerType = ownerType;
            KeyHolderId = keyHolderId;
            JumppointId = jumppointId;
        }
    }
}