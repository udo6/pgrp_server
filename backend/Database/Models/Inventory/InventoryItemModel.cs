using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

	public class InventoryItemModelConfiguration : IEntityTypeConfiguration<InventoryItemModel>
	{
		public void Configure(EntityTypeBuilder<InventoryItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_inventory_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.InventoryId).HasColumnName("inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("int(11)");
			builder.Property(x => x.Slot).HasColumnName("slot").HasColumnType("int(11)");
			builder.Property(x => x.HasAttribute).HasColumnName("has_attribute").HasColumnType("tinyint(1)");
		}
	}
}