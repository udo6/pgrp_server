using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Dealer;

namespace Database.Configurations.Dealer
{
	public class DealerModelConfiguration : IEntityTypeConfiguration<DealerModel>
	{
		public void Configure(EntityTypeBuilder<DealerModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_dealer");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}
