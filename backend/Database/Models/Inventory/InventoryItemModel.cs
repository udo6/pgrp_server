namespace Database.Models.Inventory
{
    public class InventoryItemModel
    {
        public int Id { get; set; }
        public int InventoryId { get; set; }
        public int ItemId { get; set; }
        public int Amount { get; set; }
        public int Slot { get; set; }
        public bool HasAttribute { get; set; }

        public InventoryItemModel() { }

        public InventoryItemModel(int inventoryId, int itemId, int amount, int slot, bool hasAttribute)
        {
            InventoryId = inventoryId;
            ItemId = itemId;
            Amount = amount;
            Slot = slot;
            HasAttribute = hasAttribute;
        }
    }
}