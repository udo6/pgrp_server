using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

	public class InventoryItemAttributeModelConfiguration : IEntityTypeConfiguration<InventoryItemAttributeModel>
	{
		public void Configure(EntityTypeBuilder<InventoryItemAttributeModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_inventory_item_attribute");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.InventoryItemId).HasColumnName("inventory_item_id").HasColumnType("int(11)");
			builder.Property(x => x.Value).HasColumnName("value").HasColumnType("int(11)");
		}
	}
}