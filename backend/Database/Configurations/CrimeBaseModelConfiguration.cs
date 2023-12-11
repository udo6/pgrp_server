using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Crimes;

namespace Database.Configurations
{
    public class CrimeBaseModelConfiguration : IEntityTypeConfiguration<CrimeBaseModel>
	{
		public void Configure(EntityTypeBuilder<CrimeBaseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_crimes");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.GroupId).HasColumnName("group_id").HasColumnType("int(11)");
			builder.Property(x => x.JailTime).HasColumnName("jailtime").HasColumnType("int(11)");
			builder.Property(x => x.Fine).HasColumnName("fine").HasColumnType("int(11)");
		}
	}
}