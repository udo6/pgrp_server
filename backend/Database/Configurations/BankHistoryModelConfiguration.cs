using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Bank;

namespace Database.Configurations
{
    public class BankHistoryModelConfiguration : IEntityTypeConfiguration<BankHistoryModel>
	{
		public void Configure(EntityTypeBuilder<BankHistoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_bank_history");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.BankName).HasColumnName("bank_name").HasColumnType("varchar(255)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.Withdraw).HasColumnName("withdraw").HasColumnType("tinyint(1)");
			builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("int(11)");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
		}
	}
}