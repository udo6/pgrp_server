using Logs.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Logs.Configurations
{
	public class PlayerConnectionModelConfiguration : IEntityTypeConfiguration<PlayerConnectionModel>
	{
		public void Configure(EntityTypeBuilder<PlayerConnectionModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("player_connection");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.Join).HasColumnName("join").HasColumnType("tinyint(1)");
			builder.Property(x => x.DateTime).HasColumnName("datetime").HasColumnType("datetime");
		}
	}
}