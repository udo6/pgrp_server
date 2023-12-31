using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Account
{
	public class WarnModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public string Reason { get; set; }
		public int AdminId { get; set; }
		public DateTime Date { get; set; }

		public WarnModel()
		{
			Reason = string.Empty;
		}

		public WarnModel(int accountId, string reason, int adminId, DateTime date)
		{
			AccountId = accountId;
			Reason = reason;
			AdminId = adminId;
			Date = date;
		}
	}

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
