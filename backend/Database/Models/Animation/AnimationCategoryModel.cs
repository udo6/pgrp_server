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
}