using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.VehicleShop
{
    public class VehicleShopSpawnModel
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int PositionId { get; set; }

        public VehicleShopSpawnModel()
        {
        }

        public VehicleShopSpawnModel(int shopId, int positionId)
        {
            ShopId = shopId;
            PositionId = positionId;
        }
    }

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