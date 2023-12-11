using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.VehicleShop;

namespace Database.Configurations
{
    public class VehicleShopSpawnModelConfiguration : IEntityTypeConfiguration<VehicleShopSpawnModel>
	{
		public void Configure(EntityTypeBuilder<VehicleShopSpawnModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_vehicle_shop_spawns");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ShopId).HasColumnName("shop_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}