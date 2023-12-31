using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Warehouse
{
    public class WarehouseModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public WarehouseType Type { get; set; }
        public int OwnerId { get; set; }
        public OwnerType OwnerType { get; set; }
        public int KeyHolderId { get; set; }
        public int JumppointId { get; set; }

        public WarehouseModel()
        {
        }

        public WarehouseModel(int positionId, WarehouseType type, int ownerId, OwnerType ownerType, int keyHolderId, int jumppointId)
        {
            PositionId = positionId;
            Type = type;
            OwnerId = ownerId;
            OwnerType = ownerType;
            KeyHolderId = keyHolderId;
            JumppointId = jumppointId;
        }
    }

	public class WarehouseModelConfiguration : IEntityTypeConfiguration<WarehouseModel>
	{
		public void Configure(EntityTypeBuilder<WarehouseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_warehouses");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
			builder.Property(x => x.KeyHolderId).HasColumnName("key_holder_id").HasColumnType("int(11)");
			builder.Property(x => x.JumppointId).HasColumnName("jumppoint_id").HasColumnType("int(11)");
		}
	}
}