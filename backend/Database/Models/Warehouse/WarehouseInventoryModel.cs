using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

	public class WarehouseInventoryModelConfiguration : IEntityTypeConfiguration<WarehouseInventoryModel>
	{
		public void Configure(EntityTypeBuilder<WarehouseInventoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_warehouse_inventories");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.WarehouseId).HasColumnName("warehouse_id").HasColumnType("int(11)");
			builder.Property(x => x.InventoryId).HasColumnName("inventory_id").HasColumnType("int(11)");
		}
	}
}