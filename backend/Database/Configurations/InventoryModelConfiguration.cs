using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Inventory;

namespace Database.Configurations
{
    internal class InventoryModelConfiguration : IEntityTypeConfiguration<InventoryModel>
	{
		public void Configure(EntityTypeBuilder<InventoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_inventories");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Slots).HasColumnName("slots").HasColumnType("int(11)");
			builder.Property(x => x.MaxWeight).HasColumnName("max_weight").HasColumnType("float");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}