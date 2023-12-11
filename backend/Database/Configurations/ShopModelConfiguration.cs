using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Shop;

namespace Database.Configurations
{
    public class ShopModelConfiguration : IEntityTypeConfiguration<ShopModel>
	{
		public void Configure(EntityTypeBuilder<ShopModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_shops");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.PedPositionId).HasColumnName("ped_position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
		}
	}
}