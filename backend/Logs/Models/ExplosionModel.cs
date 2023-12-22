using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Models
{
	public class ExplosionModel
	{
		public int Id { get; set; }
		public int PlayerId { get; set; }
		public int ExplosionType { get; set; }
		public DateTime Date { get; set; }

		public ExplosionModel()
		{
		}

		public ExplosionModel(int playerId, int explosionType, DateTime date)
		{
			PlayerId = playerId;
			ExplosionType = explosionType;
			Date = date;
		}
	}

	public class ExplosionModelConfiguration : IEntityTypeConfiguration<ExplosionModel>
	{
		public void Configure(EntityTypeBuilder<ExplosionModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_explosions");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PlayerId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.ExplosionType).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.Date).HasColumnName("datetime").HasColumnType("datetime");
		}
	}
}
