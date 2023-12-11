using Database.Models.FFA;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations.FFA
{
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