using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Barber;

namespace Database.Configurations
{
	public class BarberModelConfiguration : IEntityTypeConfiguration<BarberModel>
	{
		public void Configure(EntityTypeBuilder<BarberModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_barbers");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}