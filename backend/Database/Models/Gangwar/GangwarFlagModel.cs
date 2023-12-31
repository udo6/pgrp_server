using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Gangwar
{
    public class GangwarFlagModel
    {
        public int Id { get; set; }
        public int GangwarId { get; set; }
        public int PositionId { get; set; }

        public GangwarFlagModel() { }

        public GangwarFlagModel(int gangwarId, int positionId)
        {
            GangwarId = gangwarId;
            PositionId = positionId;
        }
    }

	public class GangwarFlagModelConfiguration : IEntityTypeConfiguration<GangwarFlagModel>
	{
		public void Configure(EntityTypeBuilder<GangwarFlagModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_gangwar_flags");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.GangwarId).HasColumnName("gangwar_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}