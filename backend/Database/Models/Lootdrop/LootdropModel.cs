using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Lootdrop
{
    public class LootdropModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MainPositionId { get; set; }
        public int Box1PositionId { get; set; }
        public int Box2PositionId { get; set; }
        public int Box3PositionId { get; set; }

        public LootdropModel()
        {
            Name = string.Empty;
        }

        public LootdropModel(string name, int mainPositionId, int box1PositionId, int box2PositionId, int box3PositionId)
		{
			Name = name;
			MainPositionId = mainPositionId;
            Box1PositionId = box1PositionId;
            Box2PositionId = box2PositionId;
            Box3PositionId = box3PositionId;
        }
    }

	public class LootdropModelConfiguration : IEntityTypeConfiguration<LootdropModel>
	{
		public void Configure(EntityTypeBuilder<LootdropModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_lootdrops");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.MainPositionId).HasColumnName("main_position_id").HasColumnType("int(11)");
			builder.Property(x => x.Box1PositionId).HasColumnName("box1_position_id").HasColumnType("int(11)");
			builder.Property(x => x.Box2PositionId).HasColumnName("box2_position_id").HasColumnType("int(11)");
			builder.Property(x => x.Box3PositionId).HasColumnName("box3_position_id").HasColumnType("int(11)");
		}
	}
}