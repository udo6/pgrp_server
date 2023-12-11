using Database.Models.Dealer;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations.Dealer
{
	public class DealerItemModelConfiguration : IEntityTypeConfiguration<DealerItemModel>
	{
		public void Configure(EntityTypeBuilder<DealerItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_dealer_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
		}
	}
}
