using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Inventory;

namespace Database.Configurations
{
    internal class InventoryItemModelConfiguration : IEntityTypeConfiguration<InventoryItemModel>
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