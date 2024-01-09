using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Hospital
{
	public class HospitalModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public int HeliPositionId { get; set; }

		public HospitalModel()
		{
		}

		public HospitalModel(int positionId, int heliPositionId)
		{
			PositionId = positionId;
			HeliPositionId = heliPositionId;
		}
	}

	public class HospitalModelConfiguration : IEntityTypeConfiguration<HospitalModel>
	{
		public void Configure(EntityTypeBuilder<HospitalModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_hospitals");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.HeliPositionId).HasColumnName("heli_position_id").HasColumnType("int(11)");
		}
	}
}