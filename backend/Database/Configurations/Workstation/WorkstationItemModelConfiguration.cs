using Database.Models.Workstation;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations.Workstation
{
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