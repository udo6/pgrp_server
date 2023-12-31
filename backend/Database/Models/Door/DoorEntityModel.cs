using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Door
{
	public class DoorEntityModel
	{
		public int Id { get; set; }
		public int DoorId { get; set; }
		public uint Model { get; set; }
		public int PositionId { get; set; }

		public DoorEntityModel()
		{
		}

		public DoorEntityModel(int doorId, uint model, int positionId)
		{
			DoorId = doorId;
			Model = model;
			PositionId = positionId;
		}
	}

	public class DoorEntityModelConfiguration : IEntityTypeConfiguration<DoorEntityModel>
	{
		public void Configure(EntityTypeBuilder<DoorEntityModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_door_entities");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.DoorId).HasColumnName("door_id").HasColumnType("int(11)");
			builder.Property(x => x.Model).HasColumnName("model").HasColumnType("uint(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}
