namespace Database.Models.Crimes
{
    public class CrimeBaseModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int GroupId { get; set; }
        public int JailTime { get; set; }
        public int Fine { get; set; }

        public CrimeBaseModel()
        {
            Label = string.Empty;
        }

        public CrimeBaseModel(string label, int group, int jailTime, int fine)
        {
            Label = label;
            GroupId = group;
            JailTime = jailTime;
            Fine = fine;
        }
    }
}