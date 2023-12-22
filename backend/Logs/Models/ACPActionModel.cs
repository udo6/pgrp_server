using Logs.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Logs.Models
{
	public class ACPActionModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public int TargetId { get; set; }
		public TargetType TargetType { get; set; }
		public ACPActionType ActionType { get; set; }
		public DateTime Date { get; set; }

		public ACPActionModel()
		{
		}

		public ACPActionModel(int accountId, int targetId, TargetType targetType, ACPActionType actionType, DateTime date)
		{
			AccountId = accountId;
			TargetId = targetId;
			TargetType = targetType;
			ActionType = actionType;
			Date = date;
		}
	}

	public class ACPActionModelConfiguration : IEntityTypeConfiguration<ACPActionModel>
	{
		public void Configure(EntityTypeBuilder<ACPActionModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("admin_acp_actions");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.TargetId).HasColumnName("target_id").HasColumnType("int(11)");
			builder.Property(x => x.TargetType).HasColumnName("target_type").HasColumnType("int(11)");
			builder.Property(x => x.ActionType).HasColumnName("action_type").HasColumnType("int(11)");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
		}
	}
}