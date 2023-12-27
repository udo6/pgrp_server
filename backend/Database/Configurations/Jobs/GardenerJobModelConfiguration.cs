using Database.Models.GardenerJob;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Database.Configurations.Jobs;

public class GardenerJobModelConfiguration : IEntityTypeConfiguration<GardenerJobModel>
{
    public void Configure(EntityTypeBuilder<GardenerJobModel> builder)
    {
        builder.HasKey(x => x.Id);
        builder.ToTable("server_gardener_jobs");
        builder.HasIndex(x => x.Id).HasDatabaseName("id");
        builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
        builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
        builder.Property(x => x.VehicleSpawnPositionId).HasColumnName("vehicle_spawn_position_id")
            .HasColumnType("int(11)");
    }
}