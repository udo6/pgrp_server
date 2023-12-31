using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Inventory
{
    public class ItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public int StackSize { get; set; }
        public float Weight { get; set; }

        public ItemModel()
        {
            Name = string.Empty;
            Icon = string.Empty;
        }

        public ItemModel(string name, string icon, int stackSize, float weight)
        {
            Name = name;
            Icon = icon;
            StackSize = stackSize;
            Weight = weight;
        }
    }

	public class ItemModelConfiguration : IEntityTypeConfiguration<ItemModel>
	{
		public void Configure(EntityTypeBuilder<ItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.Icon).HasColumnName("icon").HasColumnType("varchar(255)");
			builder.Property(x => x.StackSize).HasColumnName("stack_size").HasColumnType("int(11)");
			builder.Property(x => x.Weight).HasColumnName("weight").HasColumnType("float");
		}
	}
}