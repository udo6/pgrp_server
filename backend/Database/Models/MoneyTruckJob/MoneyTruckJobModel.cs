using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.MoneyTruckJob
{
    public class MoneyTruckJobModel
    {
        public int Id { get; set; }
        public int StartLocationId { get; set; }
        public int SpawnLocationId { get; set; }

		public MoneyTruckJobModel()
		{
		}

		public MoneyTruckJobModel(int id, int startLocationId, int spawnLocationId)
		{
			Id = id;
			StartLocationId = startLocationId;
			SpawnLocationId = spawnLocationId;
		}
    }

	public class MoneyTruckJobModelConfiguration : IEntityTypeConfiguration<MoneyTruckJobModel>
	{
		public void Configure(EntityTypeBuilder<MoneyTruckJobModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_money_truck_job");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.StartLocationId).HasColumnName("start_location_id").HasColumnType("int(11)");
			builder.Property(x => x.SpawnLocationId).HasColumnName("spawn_location_id").HasColumnType("int(11)");
		}
	}
}
