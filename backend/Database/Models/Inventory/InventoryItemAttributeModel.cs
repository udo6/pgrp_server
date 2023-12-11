namespace Database.Models.Inventory
{
	public class InventoryItemAttributeModel
	{
		public int Id { get; set; }
		public int InventoryItemId { get; set; }
		public int Value { get; set; }

		public InventoryItemAttributeModel()
		{
		}

		public InventoryItemAttributeModel(int inventoryItemId, int value)
		{
			InventoryItemId = inventoryItemId;
			Value = value;
		}
	}
}