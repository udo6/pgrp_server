using Database.Models.DPOS;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations.DPOS
{
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