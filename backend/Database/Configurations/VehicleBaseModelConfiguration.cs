using Database.Models.Vehicle;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations
{
    public class VehicleBaseModelConfiguration : IEntityTypeConfiguration<VehicleBaseModel>
	{
		public void Configure(EntityTypeBuilder<VehicleBaseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_vehicle_base");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.Hash).HasColumnName("hash").HasColumnType("uint(11)");
			builder.Property(x => x.TrunkWeight).HasColumnName("trunk_weight").HasColumnType("float");
			builder.Property(x => x.TrunkSlots).HasColumnName("trunk_slots").HasColumnType("int(11)");
			builder.Property(x => x.GloveBoxWeight).HasColumnName("glovebox_weight").HasColumnType("float");
			builder.Property(x => x.GloveBoxSlots).HasColumnName("glovebox_slots").HasColumnType("int(11)");
			builder.Property(x => x.MaxFuel).HasColumnName("max_fuel").HasColumnType("float");
			builder.Property(x => x.GarageType).HasColumnName("garage_type").HasColumnType("int(11)");
			builder.Property(x => x.Tax).HasColumnName("tax").HasColumnType("int(11)");
			builder.Property(x => x.Seats).HasColumnName("seats").HasColumnType("int(11)");
		}
	}
}