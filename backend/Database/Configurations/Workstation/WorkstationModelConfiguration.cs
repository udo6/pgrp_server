using Database.Models.Animation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Workstation;

namespace Database.Configurations.Workstation
{
	public class WorkstationModelConfiguration : IEntityTypeConfiguration<WorkstationModel>
	{
		public void Configure(EntityTypeBuilder<WorkstationModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_workstations");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}