using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Models
{
	public class BanModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int AdminId { get; set; }
		public string Reason { get; set; }

		public BanModel()
		{
			Reason = string.Empty;
		}

		public BanModel(int accountId, int adminId, string reason)
		{
			AccountId = accountId;
			AdminId = adminId;
			Reason = reason;
		}
	}

	public class BanModelConfiguration : IEntityTypeConfiguration<BanModel>
	{
		public void Configure(EntityTypeBuilder<BanModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("player_bans");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.AdminId).HasColumnName("admin_id").HasColumnType("int(11)");
			builder.Property(x => x.Reason).HasColumnName("reason").HasColumnType("varchar(255)");
		}
	}
}
