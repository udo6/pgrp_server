using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Jumpoint;

namespace Database.Configurations
{
    public class JumppointModelConfiguartion : IEntityTypeConfiguration<JumppointModel>
	{
		public void Configure(EntityTypeBuilder<JumppointModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_jumppoints");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.OutsidePositionId).HasColumnName("outside_position_id").HasColumnType("int(11)");
			builder.Property(x => x.OutsideDimension).HasColumnName("outside_dimension").HasColumnType("int(11)");
			builder.Property(x => x.InsidePositionId).HasColumnName("inside_position_id").HasColumnType("int(11)");
			builder.Property(x => x.InsideDimension).HasColumnName("inside_dimension").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.KeyHolderId).HasColumnName("key_holder_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
			builder.Property(x => x.Locked).HasColumnName("locked").HasColumnType("tinyint(1)");
			builder.Property(x => x.LastCrack).HasColumnName("last_crack").HasColumnType("datetime");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}