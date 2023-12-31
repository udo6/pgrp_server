using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.VehicleShop
{
    public class VehicleShopModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public int PedPositionId { get; set; }

        public VehicleShopModel()
        {
        }

        public VehicleShopModel(int positionId, int pedPositionId)
        {
            PositionId = positionId;
            PedPositionId = pedPositionId;
        }
    }

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