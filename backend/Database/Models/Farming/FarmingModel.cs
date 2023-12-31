using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Farming
{
    public class FarmingModel
    {
        public int Id { get; set; }
        public uint ObjectHash { get; set; }
        public int NeededItem { get; set; }
        public int GainItem { get; set; }
        public int MinGain { get; set; }
        public int MaxGain { get; set; }
        public string AnimationDict { get; set; }
        public string AnimationName { get; set; }

        public FarmingModel()
        {
            AnimationDict = string.Empty;
            AnimationName = string.Empty;
        }

        public FarmingModel(uint objectHash, int neededItem, int gainItem, int minGain, int maxGain, string animationDict, string animationName)
        {
            ObjectHash = objectHash;
            NeededItem = neededItem;
            GainItem = gainItem;
            MinGain = minGain;
            MaxGain = maxGain;
            AnimationDict = animationDict;
            AnimationName = animationName;
        }
    }

	public class FarmingModelConfiguration : IEntityTypeConfiguration<FarmingModel>
	{
		public void Configure(EntityTypeBuilder<FarmingModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_farmings");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ObjectHash).HasColumnName("object_hash").HasColumnType("int(11)");
			builder.Property(x => x.NeededItem).HasColumnName("needed_item").HasColumnType("int(11)");
			builder.Property(x => x.GainItem).HasColumnName("gain_item").HasColumnType("int(11)");
			builder.Property(x => x.MinGain).HasColumnName("min_gain").HasColumnType("int(11)");
			builder.Property(x => x.MaxGain).HasColumnName("max_gain").HasColumnType("int(11)");
			builder.Property(x => x.AnimationDict).HasColumnName("anim_dict").HasColumnType("varchar(255)");
			builder.Property(x => x.AnimationName).HasColumnName("anim_name").HasColumnType("varchar(255)");
		}
	}
}