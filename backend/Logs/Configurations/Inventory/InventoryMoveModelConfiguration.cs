using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Logs.Models.Inventory;

namespace Logs.Configurations.Inventory
{
	public class InventoryMoveModelConfiguration : IEntityTypeConfiguration<InventoryMoveModel>
	{
		public void Configure(EntityTypeBuilder<InventoryMoveModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("inventory_move");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.InventoryId).HasColumnName("inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.ContainerId).HasColumnName("container_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("int(11)");
			builder.Property(x => x.DateTime).HasColumnName("datetime").HasColumnType("datetime");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}