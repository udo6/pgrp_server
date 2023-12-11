using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models;

namespace Database.Configurations
{
	internal class PositionModelConfiguration : IEntityTypeConfiguration<PositionModel>
	{
		public void Configure(EntityTypeBuilder<PositionModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_positions");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.X).HasColumnName("x").HasColumnType("float");
			builder.Property(x => x.Y).HasColumnName("y").HasColumnType("float");
			builder.Property(x => x.Z).HasColumnName("z").HasColumnType("float");
			builder.Property(x => x.Roll).HasColumnName("roll").HasColumnType("float");
			builder.Property(x => x.Pitch).HasColumnName("pitch").HasColumnType("float");
			builder.Property(x => x.Yaw).HasColumnName("yaw").HasColumnType("float");
		}
	}
}