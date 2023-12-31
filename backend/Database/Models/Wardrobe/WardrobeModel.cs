using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Wardrobe
{
    public class WardrobeModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public int OwnerId { get; set; }
        public OwnerType OwnerType { get; set; }
        public int Dimension { get; set; }

        public WardrobeModel()
        {
        }

        public WardrobeModel(int positionId, int ownerId, OwnerType ownerType, int dimension)
        {
            PositionId = positionId;
            OwnerId = ownerId;
            OwnerType = ownerType;
            Dimension = dimension;
        }
    }

	public class WardrobeModelConfiguration : IEntityTypeConfiguration<WardrobeModel>
	{
		public void Configure(EntityTypeBuilder<WardrobeModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_wardrobes");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
			builder.Property(x => x.Dimension).HasColumnName("dimension").HasColumnType("int(11)");
		}
	}
}