using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Hospital;

namespace Database.Configurations.Hospital
{
	public class HospitalModelConfiguration : IEntityTypeConfiguration<HospitalModel>
	{
		public void Configure(EntityTypeBuilder<HospitalModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_hospitals");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}