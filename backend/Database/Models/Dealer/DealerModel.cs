using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Dealer
{
	public class DealerModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }
		public bool Active { get; set; }

		public DealerModel()
		{
		}

		public DealerModel(int positionId, bool active)
		{
			PositionId = positionId;
			Active = active;
		}
	}

	public class DealerModelConfiguration : IEntityTypeConfiguration<DealerModel>
	{
		public void Configure(EntityTypeBuilder<DealerModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_dealer");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Active).HasColumnName("active").HasColumnType("tinyint(1)");
		}
	}
}
