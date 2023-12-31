using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models
{
	public class ExportDealerItemModel
	{
		public int Id { get; set; }
		public int ItemId { get; set; }
		public int Price { get; set; }

		public ExportDealerItemModel()
		{
		}

		public ExportDealerItemModel(int itemId, int price)
		{
			ItemId = itemId;
			Price = price;
		}
	}

	public class ExportDealerItemModelConfiguration : IEntityTypeConfiguration<ExportDealerItemModel>
	{
		public void Configure(EntityTypeBuilder<ExportDealerItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_export_dealer_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
		}
	}
}