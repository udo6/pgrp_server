using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Lootdrop
{
    public class LootdropLootModel
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public float Probability { get; set; }

        public LootdropLootModel()
        {
        }

        public LootdropLootModel(int itemId, float probability)
        {
            ItemId = itemId;
            Probability = probability;
        }
    }

	public class LootdropLootModelConfiguration : IEntityTypeConfiguration<LootdropLootModel>
	{
		public void Configure(EntityTypeBuilder<LootdropLootModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_lootdrop_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.Probability).HasColumnName("probability").HasColumnType("float");
		}
	}
}