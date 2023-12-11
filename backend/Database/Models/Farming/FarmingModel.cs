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
}