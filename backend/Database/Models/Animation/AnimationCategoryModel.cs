using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Animation
{
    public class AnimationCategoryModel
    {
        public int Id { get; set; }
        public string Label { get; set; }

        public AnimationCategoryModel()
        {
            Label = string.Empty;
        }

        public AnimationCategoryModel(string label)
        {
            Label = label;
        }
    }

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