using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.VehicleShop;

namespace Database.Configurations
{
    public class VehicleShopModelConfiguration : IEntityTypeConfiguration<VehicleShopModel>
	{
		public void Configure(EntityTypeBuilder<VehicleShopModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_vehicle_shops");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.PedPositionId).HasColumnName("ped_position_id").HasColumnType("int(11)");
		}
	}
}