using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations.Account
{
	public class WarnModelConfiguration : IEntityTypeConfiguration<WarnModel>
	{
		public void Configure(EntityTypeBuilder<WarnModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_warns");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.Reason).HasColumnName("reason").HasColumnType("varchar(255)");
			builder.Property(x => x.AdminId).HasColumnName("admin_id").HasColumnType("int(11)");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
		}
	}
}
