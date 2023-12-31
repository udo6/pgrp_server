using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Account
{
    public class CustomizationModel
    {
        public int Id { get; set; }

        public bool Gender { get; set; }
        public int Mother { get; set; }
        public int Father { get; set; }
        public float SkinSimilarity { get; set; }
        public float ShapeSimilarity { get; set; }

        public float NoseWidth { get; set; }
        public float NoseHeight { get; set; }
        public float NoseLength { get; set; }
        public float NoseBridge { get; set; }
        public float NosePeak { get; set; }
        public float NoseMovement { get; set; }

        public float EyeHeight { get; set; }
        public float EyeWidth { get; set; }
        public float Eye { get; set; }
        public int EyeColor { get; set; }
        public int Eyebrow { get; set; }
        public int EyebrowColor { get; set; }

        public int Hair { get; set; }
        public int HairColor { get; set; }
        public int HairHighlightColor { get; set; }
        public uint HairDlc { get; set; }
        public int Beard { get; set; }
        public int BeardColor { get; set; }
        public float BeardOpacity { get; set; }

        public float NeckWidth { get; set; }
        public float LipWidth { get; set; }
        public int Age { get; set; }

        public int Makeup { get; set; }
        public int MakeupColor { get; set; }
        public float MakeupOpacity { get; set; }

        public int Blush { get; set; }
        public int BlushColor { get; set; }
        public float BlushOpacity { get; set; }

        public int Lipstick { get; set; }
        public int LipstickColor { get; set; }
        public float LipstickOpacity { get; set; }

        public bool Finished { get; set; }

        public CustomizationModel()
        {

        }

        public CustomizationModel(bool gender, int mother, int father, float skinSimilarity, float shapeSimilarity, float noseWidth, float noseHeight, float noseLength, float noseBridge, float nosePeak, float noseMovement, float eyeHeight, float eyeWidth, float eye, int eyeColor, int eyebrow, int eyebrowColor, int hair, int hairColor, int hairHighlightColor, int beard, int beardColor, float beardOpacity, float neckWidth, float lipWidth, int age, int makeup, int makeupColor, float makeupOpacity, int blush, int blushColor, float blushOpacity, int lipstick, int lipstickColor, float lipstickOpacity, bool finished)
        {
            Gender = gender;
            Mother = mother;
            Father = father;
            SkinSimilarity = skinSimilarity;
            ShapeSimilarity = shapeSimilarity;
            NoseWidth = noseWidth;
            NoseHeight = noseHeight;
            NoseLength = noseLength;
            NoseBridge = noseBridge;
            NosePeak = nosePeak;
            NoseMovement = noseMovement;
            EyeHeight = eyeHeight;
            EyeWidth = eyeWidth;
            Eye = eye;
            EyeColor = eyeColor;
            Eyebrow = eyebrow;
            EyebrowColor = eyebrowColor;
            Hair = hair;
            HairColor = hairColor;
            HairHighlightColor = hairHighlightColor;
            Beard = beard;
            BeardColor = beardColor;
            BeardOpacity = beardOpacity;
            NeckWidth = neckWidth;
            LipWidth = lipWidth;
            Age = age;
            Makeup = makeup;
            MakeupColor = makeupColor;
            MakeupOpacity = makeupOpacity;
            Blush = blush;
            BlushColor = blushColor;
            BlushOpacity = blushOpacity;
            Lipstick = lipstick;
            LipstickColor = lipstickColor;
            LipstickOpacity = lipstickOpacity;
            Finished = finished;
        }
    }

	public class CustomizationModelConfiguration : IEntityTypeConfiguration<CustomizationModel>
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
			builder.Property(x => x.HairDlc).HasColumnName("hair_dlc").HasColumnType("uint(11)");

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