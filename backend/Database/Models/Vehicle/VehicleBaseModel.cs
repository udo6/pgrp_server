using Core.Enums;

namespace Database.Models.Vehicle
{
    public class VehicleBaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public uint Hash { get; set; }
        public float TrunkWeight { get; set; }
        public int TrunkSlots { get; set; }
        public float GloveBoxWeight { get; set; }
        public int GloveBoxSlots { get; set; }
        public float MaxFuel { get; set; }
        public GarageType GarageType { get; set; }
        public int Tax { get; set; }
        public int Seats { get; set; }
        public int Price { get; set; }

        public VehicleBaseModel()
        {
            Name = string.Empty;
        }

        public VehicleBaseModel(string name, uint hash, float trunkWeight, int trunkSlots, float gloveBoxWeight, int gloveBoxSlots, float maxFuel, GarageType garageType, int tax, int seats, int price)
        {
            Name = name;
            Hash = hash;
            TrunkWeight = trunkWeight;
            TrunkSlots = trunkSlots;
            GloveBoxWeight = gloveBoxWeight;
            GloveBoxSlots = gloveBoxSlots;
            MaxFuel = maxFuel;
            GarageType = garageType;
            Tax = tax;
            Seats = seats;
            Price = price;
        }
    }
}