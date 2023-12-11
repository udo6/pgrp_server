namespace Database.Models.Phone
{
    public class PhoneContactModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public string Name { get; set; }
        public int Number { get; set; }
        public bool Favorite { get; set; }

        public PhoneContactModel()
        {
            Name = string.Empty;
        }

        public PhoneContactModel(int accountId, string name, int number, bool favorite)
        {
            AccountId = accountId;
            Name = name;
            Number = number;
            Favorite = favorite;
        }
    }
}