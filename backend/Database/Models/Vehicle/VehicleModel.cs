using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Vehicle
{
    public class VehicleModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int KeyHolderId { get; set; }
        public bool Parked { get; set; }
        public string Plate { get; set; }
        public string Note { get; set; }
        public float Fuel { get; set; }
        public OwnerType Type { get; set; }
        public int GarageId { get; set; }
        public int PositionId { get; set; }
        public int TrunkId { get; set; }
        public int GloveBoxId { get; set; }
        public int BaseId { get; set; }
        public int TuningId { get; set; }

        public VehicleModel()
        {
            Plate = string.Empty;
            Note = string.Empty;
        }

        public VehicleModel(int ownerId, int keyHolderId, bool parked, string plate, string note, float fuel, OwnerType type, int garageId, int positionId, int trunkId, int gloveBoxId, int baseId, int tuningId)
        {
            OwnerId = ownerId;
            KeyHolderId = keyHolderId;
            Parked = parked;
            Plate = plate;
            Note = note;
            Fuel = fuel;
            Type = type;
            GarageId = garageId;
            PositionId = positionId;
            TrunkId = trunkId;
            GloveBoxId = gloveBoxId;
            BaseId = baseId;
            TuningId = tuningId;
        }
    }

	public class VehicleModelConfigration : IEntityTypeConfiguration<VehicleModel>
	{
		public void Configure(EntityTypeBuilder<VehicleModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_vehicles");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.KeyHolderId).HasColumnName("key_holder_id").HasColumnType("int(11)");
			builder.Property(x => x.Parked).HasColumnName("parked").HasColumnType("tinyint(1)");
			builder.Property(x => x.Plate).HasColumnName("plate").HasColumnType("varchar(10)");
			builder.Property(x => x.Note).HasColumnName("note").HasColumnType("varchar(50)");
			builder.Property(x => x.Fuel).HasColumnName("fuel").HasColumnType("float");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.GarageId).HasColumnName("garage_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.TrunkId).HasColumnName("trunk_id").HasColumnType("int(11)");
			builder.Property(x => x.GloveBoxId).HasColumnName("glovebox_id").HasColumnType("int(11)");
			builder.Property(x => x.BaseId).HasColumnName("base_id").HasColumnType("int(11)");
			builder.Property(x => x.TuningId).HasColumnName("tuning_id").HasColumnType("int(11)");
		}
	}
}