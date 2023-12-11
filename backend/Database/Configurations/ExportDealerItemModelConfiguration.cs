using Database.Models.Animation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database.Configurations
{
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