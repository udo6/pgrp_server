namespace Database.Models.Warehouse
{
    public class WarehouseInventoryModel
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int InventoryId { get; set; }

        public WarehouseInventoryModel()
        {
        }

        public WarehouseInventoryModel(int warehouseId, int inventoryId)
        {
            WarehouseId = warehouseId;
            InventoryId = inventoryId;
        }
    }
}