using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Wardrobe;

namespace Database.Configurations
{
    public class WardrobeModelConfiguration : IEntityTypeConfiguration<WardrobeModel>
	{
		public void Configure(EntityTypeBuilder<WardrobeModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_wardrobes");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
			builder.Property(x => x.Dimension).HasColumnName("dimension").HasColumnType("int(11)");
		}
	}
}