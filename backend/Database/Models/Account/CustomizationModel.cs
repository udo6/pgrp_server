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
}