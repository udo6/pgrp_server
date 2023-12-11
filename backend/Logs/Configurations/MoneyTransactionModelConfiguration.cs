using Logs.Models.Inventory;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Logs.Models;

namespace Logs.Configurations
{
	public class MoneyTransactionModelConfiguration : IEntityTypeConfiguration<MoneyTransactionModel>
	{
		public void Configure(EntityTypeBuilder<MoneyTransactionModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("player_money");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.TargetId).HasColumnName("target_id").HasColumnType("int(11)");
			builder.Property(x => x.Amount).HasColumnName("amount").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.DateTime).HasColumnName("datetime").HasColumnType("datetime");
		}
	}
}