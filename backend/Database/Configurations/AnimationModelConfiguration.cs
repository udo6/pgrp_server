using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Animation;

namespace Database.Configurations
{
    public class AnimationModelConfiguration : IEntityTypeConfiguration<AnimationModel>
	{
		public void Configure(EntityTypeBuilder<AnimationModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_animations");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.CategoryId).HasColumnName("category_id").HasColumnType("int(11)");
			builder.Property(x => x.Dictionary).HasColumnName("dict").HasColumnType("varchar(255)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.Flags).HasColumnName("flags").HasColumnType("int(11)");
		}
	}
}