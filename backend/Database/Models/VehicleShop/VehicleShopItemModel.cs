using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.VehicleShop
{
    public class VehicleShopItemModel
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int VehicleBaseId { get; set; }
        public int PositionId { get; set; }

        public VehicleShopItemModel() { }

        public VehicleShopItemModel(int shopId, int vehicleBaseId, int positionId)
        {
            ShopId = shopId;
            VehicleBaseId = vehicleBaseId;
            PositionId = positionId;
        }
    }

	public class VehicleShopItemModelConfiguration : IEntityTypeConfiguration<VehicleShopItemModel>
	{
		public void Configure(EntityTypeBuilder<VehicleShopItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_vehicle_shop_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ShopId).HasColumnName("shop_id").HasColumnType("int(11)");
			builder.Property(x => x.VehicleBaseId).HasColumnName("base_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}