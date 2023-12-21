using Database.Models.Tattoo;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations
{
	public class TattooShopItemModelConfiguration : IEntityTypeConfiguration<TattooShopItemModel>
	{
		public void Configure(EntityTypeBuilder<TattooShopItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_tattoo_shop_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.TattooShopId).HasColumnName("tattoo_shop_id").HasColumnType("int(11)");
			builder.Property(x => x.Collection).HasColumnName("collection").HasColumnType("uint(11)");
			builder.Property(x => x.Overlay).HasColumnName("overlay").HasColumnType("uint(11)");
			builder.Property(x => x.Category).HasColumnName("category").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
		}
	}
}