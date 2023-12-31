using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Animation
{
    public class AnimationModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int CategoryId { get; set; }
        public string Dictionary { get; set; }
        public string Name { get; set; }
        public int Flags { get; set; }

        public AnimationModel()
        {
            Label = string.Empty;
            Dictionary = string.Empty;
            Name = string.Empty;
        }

        public AnimationModel(string label, int category, string dictionary, string name, int flags)
        {
            Label = label;
            CategoryId = category;
            Dictionary = dictionary;
            Name = name;
            Flags = flags;
        }
    }

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