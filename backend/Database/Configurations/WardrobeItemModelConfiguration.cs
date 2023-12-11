using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Wardrobe;

namespace Database.Configurations
{
    public class WardrobeItemModelConfiguration : IEntityTypeConfiguration<WardrobeItemModel>
	{
		public void Configure(EntityTypeBuilder<WardrobeItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_wardrobe_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.Gender).HasColumnName("gender").HasColumnType("int(11)");
			builder.Property(x => x.Component).HasColumnName("component").HasColumnType("int(11)");
			builder.Property(x => x.Drawable).HasColumnName("drawable").HasColumnType("int(11)");
			builder.Property(x => x.Texture).HasColumnName("texture").HasColumnType("int(11)");
			builder.Property(x => x.Dlc).HasColumnName("dlc").HasColumnType("int(11)");
			builder.Property(x => x.Prop).HasColumnName("prop").HasColumnType("tinyint(1)");
		}
	}
}