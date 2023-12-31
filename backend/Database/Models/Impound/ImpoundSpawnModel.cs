using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.DPOS
{
	public class ImpoundSpawnModel
	{
		public int Id { get; set; }
		public int ImpoundId { get; set; }
		public int PositionId { get; set; }

		public ImpoundSpawnModel()
		{
		}

		public ImpoundSpawnModel(int impoundId, int positionId)
		{
			ImpoundId = impoundId;
			PositionId = positionId;
		}
	}

	public class ImpoundSpawnModelConfiguration : IEntityTypeConfiguration<ImpoundSpawnModel>
	{
		public void Configure(EntityTypeBuilder<ImpoundSpawnModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_impound_spawns");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ImpoundId).HasColumnName("impound_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}