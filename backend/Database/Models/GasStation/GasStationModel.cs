using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.GasStation
{
    public class GasStationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public int Price { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }

        public GasStationModel()
        {
            Name = string.Empty;
        }

        public GasStationModel(string name, int positionId, int minPrice, int maxPrice)
        {
            Name = name;
            PositionId = positionId;
            Price = 0;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }

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