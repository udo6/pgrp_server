using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.House
{
    public class HouseModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public HouseType Type { get; set; }
        public int OwnerId { get; set; }
        public int InventoryId { get; set; }
        public int KeyHolderId { get; set; }
        public int JumppointId { get; set; }
        public int WardrobeId { get; set; }

        public HouseModel()
        {
        }

        public HouseModel(int positionId, HouseType type, int ownerId, int inventoryId, int keyHolderId, int jumppointId, int wardrobeId)
        {
            PositionId = positionId;
            Type = type;
            OwnerId = ownerId;
            InventoryId = inventoryId;
            KeyHolderId = keyHolderId;
            JumppointId = jumppointId;
            WardrobeId = wardrobeId;
        }
    }

	public class HouseModelConfiguration : IEntityTypeConfiguration<HouseModel>
	{
		public void Configure(EntityTypeBuilder<HouseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_houses");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.InventoryId).HasColumnName("inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.KeyHolderId).HasColumnName("key_holder_id").HasColumnType("int(11)");
			builder.Property(x => x.JumppointId).HasColumnName("jumppoint_id").HasColumnType("int(11)");
			builder.Property(x => x.WardrobeId).HasColumnName("wardrobe_id").HasColumnType("int(11)");
		}
	}
}