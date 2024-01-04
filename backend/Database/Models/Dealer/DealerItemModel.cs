using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Dealer
{
	public class DealerItemModel
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		public int MinPrice { get; set; }
		public int MaxPrice { get; set; }

		public DealerItemModel()
		{
		}

		public DealerItemModel(int itemId, int minPrice, int maxPrice)
		{
			ItemId = itemId;
			MinPrice = minPrice;
			MaxPrice = maxPrice;
		}
	}

	public class DealerItemModelConfiguration : IEntityTypeConfiguration<DealerItemModel>
	{
		public void Configure(EntityTypeBuilder<DealerItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_dealer_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.MinPrice).HasColumnName("min_price").HasColumnType("int(11)");
			builder.Property(x => x.MaxPrice).HasColumnName("max_price").HasColumnType("int(11)");
		}
	}
}
