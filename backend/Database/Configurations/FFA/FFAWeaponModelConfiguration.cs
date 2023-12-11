using Database.Models.FFA;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations.FFA
{
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