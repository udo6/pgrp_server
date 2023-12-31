using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Door
{
	public class DoorModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public bool Locked { get; set; }
		public float Radius { get; set; }

		public DoorModel()
		{
		}

		public DoorModel(int positionId, bool locked, float radius)
		{
			PositionId = positionId;
			Locked = locked;
			Radius = radius;
		}
	}

	public class DoorModelConfiguration : IEntityTypeConfiguration<DoorModel>
	{
		public void Configure(EntityTypeBuilder<DoorModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_doors");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Locked).HasColumnName("locked").HasColumnType("tinyint(1)");
			builder.Property(x => x.Radius).HasColumnName("radius").HasColumnType("float");
		}
	}
}
