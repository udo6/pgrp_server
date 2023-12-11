using Core.Enums;

namespace Database.Models.Vehicle
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int KeyHolderId { get; set; }
        public bool Parked { get; set; }
        public string Plate { get; set; }
        public string Note { get; set; }
        public float Fuel { get; set; }
        public OwnerType Type { get; set; }
        public int GarageId { get; set; }
        public int PositionId { get; set; }
        public int TrunkId { get; set; }
        public int GloveBoxId { get; set; }
        public int BaseId { get; set; }
        public int TuningId { get; set; }

        public VehicleModel()
        {
            Plate = string.Empty;
            Note = string.Empty;
        }

        public VehicleModel(int ownerId, int keyHolderId, bool parked, string plate, string note, float fuel, OwnerType type, int garageId, int positionId, int trunkId, int gloveBoxId, int baseId, int tuningId)
        {
            OwnerId = ownerId;
            KeyHolderId = keyHolderId;
            Parked = parked;
            Plate = plate;
            Note = note;
            Fuel = fuel;
            Type = type;
            GarageId = garageId;
            PositionId = positionId;
            TrunkId = trunkId;
            GloveBoxId = gloveBoxId;
            BaseId = baseId;
            TuningId = tuningId;
        }
    }
}