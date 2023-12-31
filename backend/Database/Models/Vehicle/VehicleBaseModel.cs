using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Vehicle
{
    public class VehicleBaseModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public uint Hash { get; set; }
        public float TrunkWeight { get; set; }
        public int TrunkSlots { get; set; }
        public float GloveBoxWeight { get; set; }
        public int GloveBoxSlots { get; set; }
        public float MaxFuel { get; set; }
        public GarageType GarageType { get; set; }
        public int Tax { get; set; }
        public int Seats { get; set; }
        public int Price { get; set; }

        public VehicleBaseModel()
        {
            Name = string.Empty;
        }

        public VehicleBaseModel(string name, uint hash, float trunkWeight, int trunkSlots, float gloveBoxWeight, int gloveBoxSlots, float maxFuel, GarageType garageType, int tax, int seats, int price)
        {
            Name = name;
            Hash = hash;
            TrunkWeight = trunkWeight;
            TrunkSlots = trunkSlots;
            GloveBoxWeight = gloveBoxWeight;
            GloveBoxSlots = gloveBoxSlots;
            MaxFuel = maxFuel;
            GarageType = garageType;
            Tax = tax;
            Seats = seats;
            Price = price;
        }
    }

	public class VehicleBaseModelConfiguration : IEntityTypeConfiguration<VehicleBaseModel>
	{
		public void Configure(EntityTypeBuilder<VehicleBaseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_vehicle_base");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.Hash).HasColumnName("hash").HasColumnType("uint(11)");
			builder.Property(x => x.TrunkWeight).HasColumnName("trunk_weight").HasColumnType("float");
			builder.Property(x => x.TrunkSlots).HasColumnName("trunk_slots").HasColumnType("int(11)");
			builder.Property(x => x.GloveBoxWeight).HasColumnName("glovebox_weight").HasColumnType("float");
			builder.Property(x => x.GloveBoxSlots).HasColumnName("glovebox_slots").HasColumnType("int(11)");
			builder.Property(x => x.MaxFuel).HasColumnName("max_fuel").HasColumnType("float");
			builder.Property(x => x.GarageType).HasColumnName("garage_type").HasColumnType("int(11)");
			builder.Property(x => x.Tax).HasColumnName("tax").HasColumnType("int(11)");
			builder.Property(x => x.Seats).HasColumnName("seats").HasColumnType("int(11)");
		}
	}
}