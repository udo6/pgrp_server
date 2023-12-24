using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Barber;

namespace Database.Configurations
{
	public class BarberBeardModelConfigration : IEntityTypeConfiguration<BarberBeardModel>
	{
		public void Configure(EntityTypeBuilder<BarberBeardModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_barber_beards");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.BarberId).HasColumnName("barber_id").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.Value).HasColumnName("value").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
		}
	}
}
