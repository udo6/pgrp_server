using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Tattoo
{
	public class TattooShopItemModel
	{
		public int Id { get; set; }
		public int TattooShopId { get; set; }
		public uint Collection { get; set; }
		public uint Overlay { get; set; }
		public int Category { get; set; }
		public string Label { get; set; }
		public int Price { get; set; }

		public TattooShopItemModel()
		{
			Label = string.Empty;
		}

		public TattooShopItemModel(int tattooShopId, uint collection, uint overlay, int category, string label, int price)
		{
			TattooShopId = tattooShopId;
			Collection = collection;
			Overlay = overlay;
			Category = category;
			Label = label;
			Price = price;
		}
	}

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