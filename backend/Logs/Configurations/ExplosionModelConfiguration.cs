using Logs.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Configurations
{
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
