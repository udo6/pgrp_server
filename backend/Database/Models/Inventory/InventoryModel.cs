using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Inventory
{
    public class InventoryModel
    {
        public int Id { get; set; }
        public int Slots { get; set; }
        public float MaxWeight { get; set; }
        public InventoryType Type { get; set; }

        public InventoryModel() { }

        public InventoryModel(int slots, float maxWeight, InventoryType type)
        {
            Slots = slots;
            MaxWeight = maxWeight;
            Type = type;
        }
    }

	public class InventoryModelConfiguration : IEntityTypeConfiguration<InventoryModel>
	{
		public void Configure(EntityTypeBuilder<InventoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_inventories");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Slots).HasColumnName("slots").HasColumnType("int(11)");
			builder.Property(x => x.MaxWeight).HasColumnName("max_weight").HasColumnType("float");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}