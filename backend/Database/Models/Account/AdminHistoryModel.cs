using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Account
{
	public class AdminHistoryModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string Reason { get; set; }
		public int AdminId { get; set; }
		public string AdminName { get; set; }
		public DateTime Date { get; set; }
		public AdminHistoryType Type { get; set; }

		public AdminHistoryModel()
		{
			Reason = string.Empty;
			AdminName = string.Empty;
		}

		public AdminHistoryModel(int accountId, string reason, int adminId, string adminName, DateTime date, AdminHistoryType type)
		{
			AccountId = accountId;
			Reason = reason;
			AdminId = adminId;
			AdminName = adminName;
			Date = date;
			Type = type;
		}
	}

	public class AdminHistoryModelConfiguration : IEntityTypeConfiguration<AdminHistoryModel>
	{
		public void Configure(EntityTypeBuilder<AdminHistoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_admin_history");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.Reason).HasColumnName("reason").HasColumnType("varchar(255)");
			builder.Property(x => x.AdminId).HasColumnName("admin_id").HasColumnType("int(11)");
			builder.Property(x => x.AdminName).HasColumnName("admin_name").HasColumnType("varchar(255)");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}
