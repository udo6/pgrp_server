using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Vehicle;

namespace Database.Configurations
{
    public class VehicleModelConfigration : IEntityTypeConfiguration<VehicleModel>
	{
		public void Configure(EntityTypeBuilder<VehicleModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_vehicles");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.KeyHolderId).HasColumnName("key_holder_id").HasColumnType("int(11)");
			builder.Property(x => x.Parked).HasColumnName("parked").HasColumnType("tinyint(1)");
			builder.Property(x => x.Plate).HasColumnName("plate").HasColumnType("varchar(10)");
			builder.Property(x => x.Note).HasColumnName("note").HasColumnType("varchar(50)");
			builder.Property(x => x.Fuel).HasColumnName("fuel").HasColumnType("float");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.GarageId).HasColumnName("garage_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.TrunkId).HasColumnName("trunk_id").HasColumnType("int(11)");
			builder.Property(x => x.GloveBoxId).HasColumnName("glovebox_id").HasColumnType("int(11)");
			builder.Property(x => x.BaseId).HasColumnName("base_id").HasColumnType("int(11)");
			builder.Property(x => x.TuningId).HasColumnName("tuning_id").HasColumnType("int(11)");
		}
	}
}