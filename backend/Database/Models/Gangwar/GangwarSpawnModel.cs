using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Gangwar
{
    public class GangwarSpawnModel
    {
        public int Id { get; set; }
        public int GangwarId { get; set; }
        public int PositionId { get; set; }
        public bool Team { get; set; }

        public GangwarSpawnModel()
        {

        }

        public GangwarSpawnModel(int gangwarId, int positionId, bool team)
        {
            GangwarId = gangwarId;
            PositionId = positionId;
            Team = team;
        }
    }

	public class GangwarSpawnModelConfiguration : IEntityTypeConfiguration<GangwarSpawnModel>
	{
		public void Configure(EntityTypeBuilder<GangwarSpawnModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_gangwar_spawns");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.GangwarId).HasColumnName("gangwar_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Team).HasColumnName("team").HasColumnType("tinyint(1)");
		}
	}
}