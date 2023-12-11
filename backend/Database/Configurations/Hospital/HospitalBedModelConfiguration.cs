using Database.Models.Hospital;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations.Hospital
{
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