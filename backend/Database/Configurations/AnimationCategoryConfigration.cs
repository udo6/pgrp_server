using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Animation;

namespace Database.Configurations
{
    public class AnimationCategoryConfigration : IEntityTypeConfiguration<AnimationCategoryModel>
	{
		public void Configure(EntityTypeBuilder<AnimationCategoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_animation_categories");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
		}
	}
}