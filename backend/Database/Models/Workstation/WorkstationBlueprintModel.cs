using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Workstation
{
    public class WorkstationBlueprintModel
    {
        public int Id { get; set; }
        public int WorkstationId { get; set; }
        public int ItemId { get; set; }
        public int ItemAmount { get; set; }
        public int Price { get; set; }
        public int NeededItem { get; set; }
        public int NeededItemAmount { get; set; }
        public int Duration { get; set; }
        public int Max { get; set; }
        public bool Active { get; set; }

        public WorkstationBlueprintModel()
        {

        }

        public WorkstationBlueprintModel(int stationId, int itemId, int itemAmount, int price, int neededItem, int neededItemAmount, int duration, int max, bool active)
        {
            WorkstationId = stationId;
            ItemId = itemId;
            ItemAmount = itemAmount;
            Price = price;
            NeededItem = neededItem;
            NeededItemAmount = neededItemAmount;
            Duration = duration;
            Max = max;
            Active = active;
        }
    }

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
			builder.Property(x => x.Active).HasColumnName("active").HasColumnType("tinyint(1)");
		}
	}
}