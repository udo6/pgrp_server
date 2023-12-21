using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Case;

namespace Database.Configurations.Case
{
	public class CaseLootModelConfiguration : IEntityTypeConfiguration<CaseLootModel>
	{
		public void Configure(EntityTypeBuilder<CaseLootModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_case_loot");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.CaseId).HasColumnName("case_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemId).HasColumnName("item_id").HasColumnType("int(11)");
			builder.Property(x => x.ItemAmount).HasColumnName("item_amount").HasColumnType("int(11)");
			builder.Property(x => x.Probability).HasColumnName("probability").HasColumnType("int(11)");
		}
	}
}
