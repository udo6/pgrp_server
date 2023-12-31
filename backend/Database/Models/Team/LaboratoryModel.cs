using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Team
{
    public class LaboratoryModel
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PositionId { get; set; }
        public int FuelInventoryId { get; set; }
        public bool Robbed { get; set; }
        public int RobInventoryId { get; set; }
        public LaboratoryType Type { get; set; }
        public DateTime LastAttack { get; set; }

        public LaboratoryModel()
        {

        }

        public LaboratoryModel(int teamId, int positionId, int fuelInventoryId, bool robbed, int robInventoryId, LaboratoryType type)
        {
            TeamId = teamId;
            PositionId = positionId;
            FuelInventoryId = fuelInventoryId;
            Robbed = robbed;
            RobInventoryId = robInventoryId;
            Type = type;
            LastAttack = DateTime.Now.AddDays(-3);
        }
    }

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