using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.House;

namespace Database.Configurations
{
    public class HouseModelConfiguration : IEntityTypeConfiguration<HouseModel>
	{
		public void Configure(EntityTypeBuilder<HouseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_houses");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.InventoryId).HasColumnName("inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.KeyHolderId).HasColumnName("key_holder_id").HasColumnType("int(11)");
			builder.Property(x => x.JumppointId).HasColumnName("jumppoint_id").HasColumnType("int(11)");
			builder.Property(x => x.WardrobeId).HasColumnName("wardrobe_id").HasColumnType("int(11)");
		}
	}
}