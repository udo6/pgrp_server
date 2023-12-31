using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.ClothesShop
{
    public class ClothesShopItemModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int Component { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }
        public uint Dlc { get; set; }
        public bool IsProp { get; set; }
        public int ShopId { get; set; }
        public int Price { get; set; }
        public int Gender { get; set; }

        public ClothesShopItemModel()
        {
            Label = string.Empty;
        }

        public ClothesShopItemModel(string label, int component, int drawable, int texture, uint dlc, bool isProp, int shopId, int price, int gender)
        {
            Label = label;
            Component = component;
            Drawable = drawable;
            Texture = texture;
            Dlc = dlc;
            IsProp = isProp;
            ShopId = shopId;
            Price = price;
            Gender = gender;
        }
    }

	public class ClothesShopItemModelConfiguration : IEntityTypeConfiguration<ClothesShopItemModel>
	{
		public void Configure(EntityTypeBuilder<ClothesShopItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_clothesshop_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.Component).HasColumnName("component").HasColumnType("int(11)");
			builder.Property(x => x.Drawable).HasColumnName("drawable").HasColumnType("int(11)");
			builder.Property(x => x.Texture).HasColumnName("texture").HasColumnType("int(11)");
			builder.Property(x => x.Dlc).HasColumnName("dlc").HasColumnType("int(11)");
			builder.Property(x => x.IsProp).HasColumnName("prop").HasColumnType("tinyint(1)");
			builder.Property(x => x.ShopId).HasColumnName("shop_id").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
			builder.Property(x => x.Gender).HasColumnName("gender").HasColumnType("int(11)");
		}
	}
}