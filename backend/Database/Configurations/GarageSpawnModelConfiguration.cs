using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Garage;

namespace Database.Configurations
{
    public class GarageSpawnModelConfiguration : IEntityTypeConfiguration<GarageSpawnModel>
	{
		public void Configure(EntityTypeBuilder<GarageSpawnModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_garage_spawns");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.GarageId).HasColumnName("garage_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}