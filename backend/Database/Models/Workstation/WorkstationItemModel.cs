using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Workstation
{
    public class WorkstationItemModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int WorkstationId { get; set; }
        public int ItemId { get; set; }
        public int ItemAmount { get; set; }
        public DateTime Added { get; set; }
        public int TimeLeft { get; set; }

        public WorkstationItemModel()
        {

        }

        public WorkstationItemModel(int accountId, int workstationId, int itemId, int itemAmount, DateTime added, int timeLeft)
        {
            AccountId = accountId;
            WorkstationId = workstationId;
            ItemId = itemId;
            ItemAmount = itemAmount;
            Added = added;
            TimeLeft = timeLeft;
        }
    }

	public class WorkstationItemModelConfiguration : IEntityTypeConfiguration<WorkstationItemModel>
	{
		public void Configure(EntityTypeBuilder<WorkstationItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_workstation_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.WorkstationId).HasColumnName("workstation_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemAmount).HasColumnName("item_amount").HasColumnType("int(11)");
			builder.Property(x => x.Added).HasColumnName("added").HasColumnType("datetime");
			builder.Property(x => x.TimeLeft).HasColumnName("time_left").HasColumnType("int(11)");
		}
	}
}