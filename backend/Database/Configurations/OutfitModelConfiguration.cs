using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Wardrobe;

namespace Database.Configurations
{
    public class OutfitModelConfiguration : IEntityTypeConfiguration<OutfitModel>
	{
		public void Configure(EntityTypeBuilder<OutfitModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_wardrobe_outfits");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.ClothesId).HasColumnName("clothes_id").HasColumnType("int(11)");
		}
	}
}