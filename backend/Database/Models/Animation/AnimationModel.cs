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
}