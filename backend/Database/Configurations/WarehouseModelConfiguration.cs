using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Warehouse;

namespace Database.Configurations
{
    public class WarehouseModelConfiguration : IEntityTypeConfiguration<WarehouseModel>
	{
		public void Configure(EntityTypeBuilder<WarehouseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_warehouses");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
			builder.Property(x => x.KeyHolderId).HasColumnName("key_holder_id").HasColumnType("int(11)");
			builder.Property(x => x.JumppointId).HasColumnName("jumppoint_id").HasColumnType("int(11)");
		}
	}
}