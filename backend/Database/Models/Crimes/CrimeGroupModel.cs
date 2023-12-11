namespace Database.Models.Crimes
{
    public class CrimeGroupModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public CrimeGroupModel()
        {
            Name = string.Empty;
        }

        public CrimeGroupModel(string name)
        {
            Name = name;
        }
    }
}