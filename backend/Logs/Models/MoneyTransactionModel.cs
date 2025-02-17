﻿using Logs.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Logs.Models
{
	public class MoneyTransactionModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int TargetId { get; set; }
		public int Amount { get; set; }
		public MoneyTransactionType Type { get; set; }
		public DateTime DateTime { get; set; }

		public MoneyTransactionModel()
		{
		}

		public MoneyTransactionModel(int accountId, int targetId, int amount, MoneyTransactionType type)
		{
			AccountId = accountId;
			TargetId = targetId;
			Amount = amount;
			Type = type;
			DateTime = DateTime.Now;
		}
	}

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