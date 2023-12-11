using Core.Enums;

namespace Database.Models.Wardrobe
{
    public class WardrobeModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public int OwnerId { get; set; }
        public OwnerType OwnerType { get; set; }
        public int Dimension { get; set; }

        public WardrobeModel()
        {
        }

        public WardrobeModel(int positionId, int ownerId, OwnerType ownerType, int dimension)
        {
            PositionId = positionId;
            OwnerId = ownerId;
            OwnerType = ownerType;
            Dimension = dimension;
        }
    }
}