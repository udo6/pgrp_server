using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.GarbageJob
{
    public class GarbageJobModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public int VehicleSpawnPositionId { get; set; }
        public int GarbageReturnPositionId { get; set; }
        public int Price { get; set; }

        [NotMapped] public int TruckCount { get; set; } = 0;
    }

	public class GarbageJobModelConfiguration : IEntityTypeConfiguration<GarbageJobModel>
	{
		public void Configure(EntityTypeBuilder<GarbageJobModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_garbage_job");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.VehicleSpawnPositionId).HasColumnName("spawn_position_id").HasColumnType("int(11)");
			builder.Property(x => x.GarbageReturnPositionId).HasColumnName("return_position_id").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
		}
	}
}
