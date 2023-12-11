using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Crimes;

namespace Database.Configurations
{
    public class CrimeGroupModelConfiguration : IEntityTypeConfiguration<CrimeGroupModel>
	{
		public void Configure(EntityTypeBuilder<CrimeGroupModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_crime_groups");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
		}
	}
}