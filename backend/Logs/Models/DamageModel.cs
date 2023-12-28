using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Logs.Models
{
	public class DamageModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int TargetId { get; set; }
		public uint Weapon {  get; set; }
		public int Damage { get; set; }
		public int BodyPart {  get; set; }
		public DateTime Date { get; set; }

		public DamageModel()
		{
		}

		public DamageModel(int accountId, int targetId, uint weapon, int damage, int bodyPart, DateTime date)
		{
			AccountId = accountId;
			TargetId = targetId;
			Weapon = weapon;
			Damage = damage;
			BodyPart = bodyPart;
			Date = date;
		}
	}

	public class DamageModelConfiguration : IEntityTypeConfiguration<DamageModel>
	{
		public void Configure(EntityTypeBuilder<DamageModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("player_damage");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.TargetId).HasColumnName("target_id").HasColumnType("int(11)");
			builder.Property(x => x.Weapon).HasColumnName("weapon").HasColumnType("uint(11)");
			builder.Property(x => x.Damage).HasColumnName("damage").HasColumnType("int(11)");
			builder.Property(x => x.BodyPart).HasColumnName("body_part").HasColumnType("int(11)");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
		}
	}
}
