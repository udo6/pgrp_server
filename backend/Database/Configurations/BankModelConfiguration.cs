using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Bank;

namespace Database.Configurations
{
    public class BankModelConfiguration : IEntityTypeConfiguration<BankModel>
	{
		public void Configure(EntityTypeBuilder<BankModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_banks");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}