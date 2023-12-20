using Core.Enums;

namespace Database.Models.Account
{
    public class LoadoutModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public uint Hash { get; set; }
        public int Ammo { get; set; }
        public int TintIndex { get; set; }
        public LoadoutType Type { get; set; }

        public LoadoutModel() { }

        public LoadoutModel(int accountId, uint hash, int ammo, int tintIndex, LoadoutType type)
        {
            AccountId = accountId;
            Hash = hash;
            Ammo = ammo;
            TintIndex = tintIndex;
            Type = type;
        }
    }
}