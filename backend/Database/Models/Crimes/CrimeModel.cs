namespace Database.Models.Crimes
{
    public class CrimeModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CrimeId { get; set; }
        public string Created { get; set; }
        public string OfficerName { get; set; }

        public CrimeModel()
        {
            Created = string.Empty;
            OfficerName = string.Empty;
        }

        public CrimeModel(int accountId, int crimeId, string created, string officerName)
        {
            AccountId = accountId;
            CrimeId = crimeId;
            Created = created;
            OfficerName = officerName;
        }
    }
}