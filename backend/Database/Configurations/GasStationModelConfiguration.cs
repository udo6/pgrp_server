using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.GasStation;

namespace Database.Configurations
{
    public class GasStationModelConfiguration : IEntityTypeConfiguration<GasStationModel>
	{
		public void Configure(EntityTypeBuilder<GasStationModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_gas_stations");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
			builder.Property(x => x.MinPrice).HasColumnName("min_price").HasColumnType("int(11)");
			builder.Property(x => x.MaxPrice).HasColumnName("max_price").HasColumnType("int(11)");
		}
	}
}