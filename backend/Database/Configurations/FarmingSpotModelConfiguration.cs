using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Farming;

namespace Database.Configurations
{
    public class FarmingSpotModelConfiguration : IEntityTypeConfiguration<FarmingSpotModel>
	{
		public void Configure(EntityTypeBuilder<FarmingSpotModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_farming_spots");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.FarmingId).HasColumnName("farming_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}