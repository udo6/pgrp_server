using AltV.Net.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models
{
	public class PositionModel
	{
		public int Id { get; set; }
		public float X { get; set; }
		public float Y { get; set; }
		public float Z { get; set; }
		public float Roll { get; set; }
		public float Pitch { get; set; }
		public float Yaw { get; set; }

		[NotMapped]
		public Position Position
		{
			get => new(X, Y, Z);
			set
			{
				X = value.X;
				Y = value.Y;
				Z = value.Z;
			}
		}

		[NotMapped]
		public Rotation Rotation
		{
			get => new(Roll, Pitch, Yaw);
			set
			{
				Roll = value.Roll;
				Pitch = value.Pitch;
				Yaw = value.Yaw;
			}
		}

		public PositionModel()
		{
		}

		public PositionModel(float x, float y, float z, float roll, float pitch, float yaw)
		{
			X = x;
			Y = y;
			Z = z;
			Roll = roll;
			Pitch = pitch;
			Yaw = yaw;
		}

		public PositionModel(Position pos)
		{
			X = pos.X;
			Y = pos.Y;
			Z = pos.Z;
			Roll = 0;
			Pitch = 0;
			Yaw = 0;
		}

		public PositionModel(Position pos, float roll, float pitch, float yaw)
		{
			X = pos.X;
			Y = pos.Y;
			Z = pos.Z;
			Roll = roll;
			Pitch = pitch;
			Yaw = yaw;
		}

		public PositionModel(Position pos, Rotation rot)
		{
			X = pos.X;
			Y = pos.Y;
			Z = pos.Z;
			Roll = rot.Roll;
			Pitch = rot.Pitch;
			Yaw = rot.Yaw;
		}
	}

	public class PositionModelConfiguration : IEntityTypeConfiguration<PositionModel>
	{
		public void Configure(EntityTypeBuilder<PositionModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_positions");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.X).HasColumnName("x").HasColumnType("float");
			builder.Property(x => x.Y).HasColumnName("y").HasColumnType("float");
			builder.Property(x => x.Z).HasColumnName("z").HasColumnType("float");
			builder.Property(x => x.Roll).HasColumnName("roll").HasColumnType("float");
			builder.Property(x => x.Pitch).HasColumnName("pitch").HasColumnType("float");
			builder.Property(x => x.Yaw).HasColumnName("yaw").HasColumnType("float");
		}
	}
}