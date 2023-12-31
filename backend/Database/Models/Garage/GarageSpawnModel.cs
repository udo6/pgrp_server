using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Garage
{
    public class GarageSpawnModel
    {
        public int Id { get; set; }
        public int GarageId { get; set; }
        public int PositionId { get; set; }

        public GarageSpawnModel()
        {

        }

        public GarageSpawnModel(int garageId, int positionId)
        {
            GarageId = garageId;
            PositionId = positionId;
        }
    }

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