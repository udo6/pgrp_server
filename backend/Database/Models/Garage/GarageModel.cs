using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Garage
{
    public class GarageModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public int PedPositionId { get; set; }
        public GarageType Type { get; set; }
        public int Owner { get; set; }
        public OwnerType OwnerType { get; set; }

        public GarageModel()
        {
            Name = string.Empty;
        }

        public GarageModel(string name, int positionId, int pedPositionId, GarageType type, int owner, OwnerType ownerType)
        {
            Name = name;
            PositionId = positionId;
            PedPositionId = pedPositionId;
            Type = type;
            Owner = owner;
            OwnerType = ownerType;
        }
    }

	public class GarageModelConfiguration : IEntityTypeConfiguration<GarageModel>
	{
		public void Configure(EntityTypeBuilder<GarageModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_garages");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.PedPositionId).HasColumnName("ped_position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.Owner).HasColumnName("owner").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
		}
	}
}