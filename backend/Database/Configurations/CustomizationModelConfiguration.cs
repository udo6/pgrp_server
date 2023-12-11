using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Account;

namespace Database.Configurations
{
    internal class CustomizationModelConfiguration : IEntityTypeConfiguration<CustomizationModel>
	{
		public void Configure(EntityTypeBuilder<CustomizationModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_customizations");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Gender).HasColumnName("gender").HasColumnType("tinyint(1)");

			builder.Property(x => x.Mother).HasColumnName("mother").HasColumnType("int(11)");
			builder.Property(x => x.Father).HasColumnName("father").HasColumnType("int(11)");
			builder.Property(x => x.ShapeSimilarity).HasColumnName("shape_similarity").HasColumnType("float");
			builder.Property(x => x.SkinSimilarity).HasColumnName("skin_similarity").HasColumnType("float");

			builder.Property(x => x.NoseWidth).HasColumnName("nose_width").HasColumnType("float");
			builder.Property(x => x.NoseHeight).HasColumnName("nose_height").HasColumnType("float");
			builder.Property(x => x.NoseLength).HasColumnName("nose_length").HasColumnType("float");
			builder.Property(x => x.NoseBridge).HasColumnName("nose_bridge").HasColumnType("float");
			builder.Property(x => x.NosePeak).HasColumnName("nose_peak").HasColumnType("float");
			builder.Property(x => x.NoseMovement).HasColumnName("nose_movement").HasColumnType("float");

			builder.Property(x => x.EyeHeight).HasColumnName("eye_height").HasColumnType("float");
			builder.Property(x => x.EyeWidth).HasColumnName("eye_width").HasColumnType("float");
			builder.Property(x => x.Eye).HasColumnName("eye").HasColumnType("float");
			builder.Property(x => x.EyeColor).HasColumnName("eye_color").HasColumnType("int(11)");
			builder.Property(x => x.Eyebrow).HasColumnName("eyebrow").HasColumnType("int(11)");
			builder.Property(x => x.EyebrowColor).HasColumnName("eyebrow_color").HasColumnType("int(11)");

			builder.Property(x => x.Hair).HasColumnName("hair").HasColumnType("int(11)");
			builder.Property(x => x.HairColor).HasColumnName("hair_color").HasColumnType("int(11)");
			builder.Property(x => x.HairHighlightColor).HasColumnName("hair_highlight_color").HasColumnType("int(11)");

			builder.Property(x => x.Beard).HasColumnName("beard").HasColumnType("int(11)");
			builder.Property(x => x.BeardColor).HasColumnName("beard_color").HasColumnType("int(11)");
			builder.Property(x => x.BeardOpacity).HasColumnName("beard_opacity").HasColumnType("float");

			builder.Property(x => x.NeckWidth).HasColumnName("neck_width").HasColumnType("float");
			builder.Property(x => x.LipWidth).HasColumnName("lip_width").HasColumnType("float");

			builder.Property(x => x.Age).HasColumnName("age").HasColumnType("int(11)");

			builder.Property(x => x.Makeup).HasColumnName("makeup").HasColumnType("int(11)");
			builder.Property(x => x.MakeupColor).HasColumnName("makeup_color").HasColumnType("int(11)");
			builder.Property(x => x.MakeupOpacity).HasColumnName("makeup_opacity").HasColumnType("float");

			builder.Property(x => x.Blush).HasColumnName("blush").HasColumnType("int(11)");
			builder.Property(x => x.BlushColor).HasColumnName("blush_color").HasColumnType("int(11)");
			builder.Property(x => x.BlushOpacity).HasColumnName("blush_opacity").HasColumnType("float");

			builder.Property(x => x.Lipstick).HasColumnName("lipstick").HasColumnType("int(11)");
			builder.Property(x => x.LipstickColor).HasColumnName("lipstick_color").HasColumnType("int(11)");
			builder.Property(x => x.LipstickOpacity).HasColumnName("lipstick_opacity").HasColumnType("float");

			builder.Property(x => x.Finished).HasColumnName("finished").HasColumnType("tinyint(1)");
		}
	}
}