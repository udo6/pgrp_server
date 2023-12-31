using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Processor
{
    public class ProcessorModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public int InputItem { get; set; }
        public int InputStepAmount { get; set; }
        public int OutputItem { get; set; }
        public int OutputStepAmount { get; set; }
        public int Time { get; set; }
        public uint PedModel { get; set; }

        public ProcessorModel() { }

        public ProcessorModel(int positionId, int inputItem, int inputStepAmount, int outputItem, int outputStepAmount, int time, uint pedModel)
        {
            PositionId = positionId;
            InputItem = inputItem;
            InputStepAmount = inputStepAmount;
            OutputItem = outputItem;
            OutputStepAmount = outputStepAmount;
            Time = time;
            PedModel = pedModel;
        }
    }

	public class ProcessorModelConfiguration : IEntityTypeConfiguration<ProcessorModel>
	{
		public void Configure(EntityTypeBuilder<ProcessorModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_processors");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.InputItem).HasColumnName("input_item_id").HasColumnType("int(11)");
			builder.Property(x => x.InputStepAmount).HasColumnName("input_item_amount").HasColumnType("int(11)");
			builder.Property(x => x.OutputItem).HasColumnName("output_item_id").HasColumnType("int(11)");
			builder.Property(x => x.OutputStepAmount).HasColumnName("output_item_amount").HasColumnType("int(11)");
			builder.Property(x => x.Time).HasColumnName("time").HasColumnType("int(11)");
			builder.Property(x => x.PedModel).HasColumnName("ped_model").HasColumnType("uint(11)");
		}
	}
}