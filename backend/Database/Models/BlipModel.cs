using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models
{
	public class BlipModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int PositionId { get; set; }
		public int Sprite { get; set; }
		public int Color { get; set; }
		public bool ShortRange { get; set; }

		public BlipModel()
		{
			Name = string.Empty;
		}

		public BlipModel(string name, int positionId, int sprite, int color, bool shortRange)
		{
			Name = name;
			PositionId = positionId;
			Sprite = sprite;
			Color = color;
			ShortRange = shortRange;
		}
	}

	public class BlipModelConfiguration : IEntityTypeConfiguration<BlipModel>
	{
		public void Configure(EntityTypeBuilder<BlipModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_blips");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Sprite).HasColumnName("sprite").HasColumnType("int(11)");
			builder.Property(x => x.Color).HasColumnName("color").HasColumnType("int(11)");
			builder.Property(x => x.ShortRange).HasColumnName("short_range").HasColumnType("tinyint(1)");
		}
	}
}