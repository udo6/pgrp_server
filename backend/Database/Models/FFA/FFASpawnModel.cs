using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.FFA
{
	public class FFASpawnModel
	{
		public int Id { get; set; }
		public int FFAId { get; set; }
		public int PositionId { get; set; }

		public FFASpawnModel()
		{
		}

		public FFASpawnModel(int fFAId, int positionId)
		{
			FFAId = fFAId;
			PositionId = positionId;
		}
	}

	public class FFASpawnModelConfiguration : IEntityTypeConfiguration<FFASpawnModel>
	{
		public void Configure(EntityTypeBuilder<FFASpawnModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_ffa_spawns");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.FFAId).HasColumnName("ffa_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}