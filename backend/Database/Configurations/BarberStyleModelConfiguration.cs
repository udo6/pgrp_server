using Database.Models.Barber;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations
{
	public class BarberStyleModelConfiguration : IEntityTypeConfiguration<BarberStyleModel>
	{
		public void Configure(EntityTypeBuilder<BarberStyleModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_barber_styles");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.BarberId).HasColumnName("barber_id").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.Value).HasColumnName("value").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
		}
	}
}