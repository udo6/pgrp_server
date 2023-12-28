using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Logs.Models
{
	public class KillModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int KillerId { get; set; }
		public uint Weapon { get; set; }
		public DateTime Date { get; set; }

		public KillModel()
		{
		}

		public KillModel(int accountId, int killerId, uint weapon, DateTime date)
		{
			AccountId = accountId;
			KillerId = killerId;
			Weapon = weapon;
			Date = date;
		}
	}

	public class KillModelConfiguration : IEntityTypeConfiguration<KillModel>
	{
		public void Configure(EntityTypeBuilder<KillModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("player_kills");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.KillerId).HasColumnName("killer_id").HasColumnType("int(11)");
			builder.Property(x => x.Weapon).HasColumnName("weapon").HasColumnType("uint(11)");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
		}
	}
}
