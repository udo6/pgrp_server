﻿using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Bank
{
    public class BankHistoryModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string BankName { get; set; }
        public TransactionType Type { get; set; }
        public bool Withdraw { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public BankHistoryModel()
        {
            Name = string.Empty;
            BankName = string.Empty;
        }

        public BankHistoryModel(int accountId, string name, string bankName, TransactionType type, bool withdraw, int amount, DateTime date)
        {
            AccountId = accountId;
            Name = name;
            BankName = bankName;
            Type = type;
            Withdraw = withdraw;
            Amount = amount;
            Date = date;
        }
    }

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