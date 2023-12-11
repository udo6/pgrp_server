namespace Database.Models.Account
{
    public class LoadoutAttatchmentModel
    {
        public int Id { get; set; }
        public int LoadoutId { get; set; }
        public uint Hash { get; set; }

        public LoadoutAttatchmentModel()
        {

        }

        public LoadoutAttatchmentModel(int loadoutId, uint hash)
        {
            LoadoutId = loadoutId;
            Hash = hash;
        }
    }
}