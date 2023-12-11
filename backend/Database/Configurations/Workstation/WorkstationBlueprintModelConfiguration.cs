using Database.Models.Workstation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations.Workstation
{
	public class WorkstationBlueprintModelConfiguration : IEntityTypeConfiguration<WorkstationBlueprintModel>
	{
		public void Configure(EntityTypeBuilder<WorkstationBlueprintModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_workstation_blueprints");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.WorkstationId).HasColumnName("workstation_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemAmount).HasColumnName("item_amount").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
			builder.Property(x => x.NeededItem).HasColumnName("needed_item").HasColumnType("int(11)");
			builder.Property(x => x.NeededItemAmount).HasColumnName("needed_item_amount").HasColumnType("int(11)");
			builder.Property(x => x.Duration).HasColumnName("duration").HasColumnType("int(11)");
			builder.Property(x => x.Max).HasColumnName("max").HasColumnType("int(11)");
		}
	}
}