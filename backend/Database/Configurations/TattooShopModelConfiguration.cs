using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Tattoo;

namespace Database.Configurations
{
	public class TattooShopModelConfiguration : IEntityTypeConfiguration<TattooShopModel>
	{
		public void Configure(EntityTypeBuilder<TattooShopModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_tattoo_shops");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}