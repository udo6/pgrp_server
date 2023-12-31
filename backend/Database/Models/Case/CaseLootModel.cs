using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Case
{
	public class CaseLootModel
	{
		public int Id { get; set; }
		public int CaseId { get; set; }
		public int ItemId { get; set; }
		public int ItemAmount { get; set; }
		public float Probability { get; set; }

		public CaseLootModel()
		{
		}

		public CaseLootModel(int caseId, int itemId, int itemAmount, float probability)
		{
			CaseId = caseId;
			ItemId = itemId;
			ItemAmount = itemAmount;
			Probability = probability;
		}
	}

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
			builder.Property(x => x.Probability).HasColumnName("probability").HasColumnType("float");
		}
	}
}
