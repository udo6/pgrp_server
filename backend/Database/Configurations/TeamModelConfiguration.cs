using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Team;

namespace Database.Configurations
{
    public class TeamModelConfiguration : IEntityTypeConfiguration<TeamModel>
	{
		public void Configure(EntityTypeBuilder<TeamModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_teams");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.ShortName).HasColumnName("short_name").HasColumnType("varchar(50)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.ColorR).HasColumnName("color_r").HasColumnType("int(11)");
			builder.Property(x => x.ColorG).HasColumnName("color_g").HasColumnType("int(11)");
			builder.Property(x => x.ColorB).HasColumnName("color_b").HasColumnType("int(11)");
			builder.Property(x => x.BlipColor).HasColumnName("blip_color").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.Warns).HasColumnName("warns").HasColumnType("int(11)");
			builder.Property(x => x.MeeleWeapon).HasColumnName("meele_name").HasColumnType("varchar(255)");
			builder.Property(x => x.MeeleWeaponHash).HasColumnName("meele_hash").HasColumnType("int(11)");
			builder.Property(x => x.Money).HasColumnName("money").HasColumnType("int(11)");
		}
	}
}