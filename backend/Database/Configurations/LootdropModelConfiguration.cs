using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Lootdrop;

namespace Database.Configurations
{
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