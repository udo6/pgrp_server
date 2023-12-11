using Core.Enums;

namespace Database.Models.Inventory
{
    public class InventoryModel
    {
        public int Id { get; set; }
        public int Slots { get; set; }
        public float MaxWeight { get; set; }
        public InventoryType Type { get; set; }

        public InventoryModel() { }

        public InventoryModel(int slots, float maxWeight, InventoryType type)
        {
            Slots = slots;
            MaxWeight = maxWeight;
            Type = type;
        }
    }
}