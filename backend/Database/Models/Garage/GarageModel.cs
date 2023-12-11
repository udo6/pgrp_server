using Core.Enums;

namespace Database.Models.Garage
{
    public class GarageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public int PedPositionId { get; set; }
        public GarageType Type { get; set; }
        public int Owner { get; set; }
        public OwnerType OwnerType { get; set; }

        public GarageModel()
        {
            Name = string.Empty;
        }

        public GarageModel(string name, int positionId, int pedPositionId, GarageType type, int owner, OwnerType ownerType)
        {
            Name = name;
            PositionId = positionId;
            PedPositionId = pedPositionId;
            Type = type;
            Owner = owner;
            OwnerType = ownerType;
        }
    }
}