using AltV.Net.Data;
using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Team
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int PositionId { get; set; }

        public byte ColorR { get; set; }
        public byte ColorG { get; set; }
        public byte ColorB { get; set; }
        [NotMapped]
        public Rgba Color => new(ColorR, ColorG, ColorB, 255);

        public byte BlipColor { get; set; }
        public TeamType Type { get; set; }
        public int Warns { get; set; }
        public string MeeleWeapon { get; set; }
        public uint MeeleWeaponHash { get; set; }
        public int Money { get; set; }

        public TeamModel()
        {
            Name = string.Empty;
            ShortName = string.Empty;
            MeeleWeapon = string.Empty;
        }

        public TeamModel(string name, string shortName, int positionId, byte colorR, byte colorG, byte colorB, byte blipColor, TeamType type, int warns, string meeleWeapon, uint meeleWeaponHash, int money)
        {
            Name = name;
            ShortName = shortName;
            PositionId = positionId;
            ColorR = colorR;
            ColorG = colorG;
            ColorB = colorB;
            BlipColor = blipColor;
            Type = type;
            Warns = warns;
            MeeleWeapon = meeleWeapon;
            MeeleWeaponHash = meeleWeaponHash;
            Money = money;
        }
    }

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