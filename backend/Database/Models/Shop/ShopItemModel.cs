using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Shop
{
    public class ShopItemModel
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int ItemId { get; set; }
        public int Price { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }
        public int MinRank { get; set; }

        public ShopItemModel()
        {
        }

        public ShopItemModel(int shopId, int itemId, int price, int minPrice, int maxPrice, int minRank)
        {
            ShopId = shopId;
            ItemId = itemId;
            Price = price;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
            MinRank = minRank;
        }
    }

	public class ShopItemModelConfiguration : IEntityTypeConfiguration<ShopItemModel>
	{
		public void Configure(EntityTypeBuilder<ShopItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_shop_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ShopId).HasColumnName("shop_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
			builder.Property(x => x.MinPrice).HasColumnName("min_price").HasColumnType("int(11)");
			builder.Property(x => x.MaxPrice).HasColumnName("max_price").HasColumnType("int(11)");
			builder.Property(x => x.MinRank).HasColumnName("min_rank").HasColumnType("int(11)");
		}
	}
}