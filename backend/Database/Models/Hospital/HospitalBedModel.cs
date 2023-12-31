using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Hospital
{
	public class HospitalBedModel
	{
		public int Id { get; set; }
		public int HospitalId { get; set; }
		public int PositionId { get; set; }

		public HospitalBedModel()
		{
		}

		public HospitalBedModel(int hospitalId, int positionId)
		{
			HospitalId = hospitalId;
			PositionId = positionId;
		}
	}

	public class HospitalBedModelConfiguration : IEntityTypeConfiguration<HospitalBedModel>
	{
		public void Configure(EntityTypeBuilder<HospitalBedModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_hospital_beds");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.HospitalId).HasColumnName("hospital_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}