using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models
{
	public class ReportModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int LastAttackerId { get; set; }
		public DateTime Date { get; set; }

		public ReportModel()
		{
		}

		public ReportModel(int accountId, int lastAttackerId, DateTime date)
		{
			AccountId = accountId;
			LastAttackerId = lastAttackerId;
			Date = date;
		}
	}

	public class ReportModelConfiguration : IEntityTypeConfiguration<ReportModel>
	{
		public void Configure(EntityTypeBuilder<ReportModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_reports");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.LastAttackerId).HasColumnName("last_attacker_id").HasColumnType("int(11)");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
		}
	}
}
