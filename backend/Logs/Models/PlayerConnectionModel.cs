using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Logs.Models
{
	public class PlayerConnectionModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public bool Join { get; set; }
		public DateTime DateTime { get; set; }

		public PlayerConnectionModel()
		{
		}

		public PlayerConnectionModel(int accountId, bool join)
		{
			AccountId = accountId;
			Join = join;
			DateTime = DateTime.Now;
		}
	}

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