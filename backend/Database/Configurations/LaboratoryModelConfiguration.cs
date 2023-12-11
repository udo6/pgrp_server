using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Team;

namespace Database.Configurations
{
    public class LaboratoryModelConfiguration : IEntityTypeConfiguration<LaboratoryModel>
	{
		public void Configure(EntityTypeBuilder<LaboratoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_team_laboratories");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.TeamId).HasColumnName("team_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.FuelInventoryId).HasColumnName("fuel_inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.RobInventoryId).HasColumnName("rob_inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.Robbed).HasColumnName("robbed").HasColumnType("tinyint(1)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.LastAttack).HasColumnName("last_attack").HasColumnType("datetime");
		}
	}
}