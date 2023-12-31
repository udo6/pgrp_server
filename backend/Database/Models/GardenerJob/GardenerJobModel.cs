using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.GardenerJob
{
	public class GardenerJobModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public int VehicleSpawnPositionId { get; set; }

		public GardenerJobModel() { }
	}

	public class GardenerJobModelConfiguration : IEntityTypeConfiguration<GardenerJobModel>
	{
		public void Configure(EntityTypeBuilder<GardenerJobModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_gardener_jobs");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.VehicleSpawnPositionId).HasColumnName("vehicle_spawn_position_id").HasColumnType("int(11)");
		}
	}
}