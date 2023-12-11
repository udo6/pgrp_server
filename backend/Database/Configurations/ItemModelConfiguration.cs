using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Inventory;

namespace Database.Configurations
{
    internal class ItemModelConfiguration : IEntityTypeConfiguration<ItemModel>
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