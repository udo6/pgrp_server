using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Processor;

namespace Database.Configurations
{
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
		}
	}
}