using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.FFA
{
	public class FFAWeaponModel
	{
		public int Id { get; set; }
		public int FFAId { get; set; }
		public uint WeaponHash { get; set; }

		public FFAWeaponModel()
		{
		}

		public FFAWeaponModel(int fFAId, uint weaponHash)
		{
			FFAId = fFAId;
			WeaponHash = weaponHash;
		}
	}

	public class FFAWeaponModelConfiguration : IEntityTypeConfiguration<FFAWeaponModel>
	{
		public void Configure(EntityTypeBuilder<FFAWeaponModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_ffa_weapons");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.FFAId).HasColumnName("ffa_id").HasColumnType("int(11)");
			builder.Property(x => x.WeaponHash).HasColumnName("weapon_hash").HasColumnType("uint(11)");
		}
	}
}