using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Workstation
{
    public class WorkstationModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public WorkstationType Type { get; set; }
        public int MaxActiveItems { get; set; }
        public uint PedModel { get; set; }

        public WorkstationModel() { }

        public WorkstationModel(int positionId, WorkstationType type, int maxActiveItems, uint pedModel)
        {
            PositionId = positionId;
            Type = type;
            MaxActiveItems = maxActiveItems;
            PedModel = pedModel;
        }
    }

	public class WorkstationModelConfiguration : IEntityTypeConfiguration<WorkstationModel>
	{
		public void Configure(EntityTypeBuilder<WorkstationModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_workstations");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
			builder.Property(x => x.MaxActiveItems).HasColumnName("max_active").HasColumnType("int(11)");
			builder.Property(x => x.PedModel).HasColumnName("ped_model").HasColumnType("uint(11)");
		}
	}
}