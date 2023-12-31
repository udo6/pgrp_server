using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Shop
{
    public class ShopModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public int PedPositionId { get; set; }
        public ShopType Type { get; set; }
        public int OwnerId { get; set; }

        public ShopModel()
        {
            Name = string.Empty;
        }

        public ShopModel(string name, int positionId, int pedPosId, ShopType type, int ownerId)
        {
            Name = name;
            PositionId = positionId;
            PedPositionId = pedPosId;
            Type = type;
            OwnerId = ownerId;
        }
    }

	public class ShopModelConfiguration : IEntityTypeConfiguration<ShopModel>
	{
		public void Configure(EntityTypeBuilder<ShopModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_shops");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.PedPositionId).HasColumnName("ped_position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
		}
	}
}