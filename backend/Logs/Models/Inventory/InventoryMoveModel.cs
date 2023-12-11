using Logs.Enums;

namespace Logs.Models.Inventory
{
    public class InventoryMoveModel
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int ContainerId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public DateTime DateTime { get; set; }
        public InventoryMoveType Type { get; set; }

        public InventoryMoveModel()
        {
        }

        public InventoryMoveModel(int inventoryId, int containerId, int itemId, int amount, InventoryMoveType type)
        {
            InventoryId = inventoryId;
            ContainerId = containerId;
            ItemId = itemId;
            Amount = amount;
            DateTime = DateTime.Now;
            Type = type;
        }
    }
}