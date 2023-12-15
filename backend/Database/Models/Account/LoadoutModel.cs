using Core.Enums;

namespace Database.Models.Account
{
    public class LoadoutModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public uint Hash { get; set; }
        public int Ammo { get; set; }
        public LoadoutType Type { get; set; }

        public LoadoutModel() { }

        public LoadoutModel(int accountId, uint hash, int ammo, LoadoutType type)
        {
            AccountId = accountId;
            Hash = hash;
            Ammo = ammo;
            Type = type;
        }
    }
}