using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logs.Models
{
	public class MagicBulletModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public uint Weapon { get; set; }
		public int Damage { get; set; }
		public float Distance { get; set; }
		public DateTime Date { get; set; }

		public MagicBulletModel()
		{
		}

		public MagicBulletModel(int accountId, uint weapon, int damage, float distance, DateTime date)
		{
			AccountId = accountId;
			Weapon = weapon;
			Damage = damage;
			Distance = distance;
			Date = date;
		}
	}

	public class MagicBulletModelConfiguration : IEntityTypeConfiguration<MagicBulletModel>
	{
		public void Configure(EntityTypeBuilder<MagicBulletModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("player_magic_bullet");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.Weapon).HasColumnName("weapon").HasColumnType("uint(11)");
			builder.Property(x => x.Damage).HasColumnName("damage").HasColumnType("int(11)");
			builder.Property(x => x.Distance).HasColumnName("distance").HasColumnType("float");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
		}
	}
}
