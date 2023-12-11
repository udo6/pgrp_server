using Core.Enums;

namespace Database.Models.Account
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ulong SocialclubId { get; set; }
        public ulong HardwareId { get; set; }
        public ulong HardwareIdEx { get; set; }
        public long DiscordId { get; set; }
        public bool Whitelisted { get; set; }
        public int Money { get; set; }
        public int BankMoney { get; set; }
        public ushort Health { get; set; }
        public ushort Armor { get; set; }
        public int ArmorItemId { get; set; }
        public InjuryType InjuryType { get; set; }
        public bool Alive { get; set; }
        public bool Coma { get; set; }
        public bool Stabilized { get; set; }
        public AdminRank AdminRank { get; set; }
        public DateTime LastOnline { get; set; }
        public int Hunger { get; set; }
        public int Thirst { get; set; }
        public bool Cuffed { get; set; }
        public bool Roped { get; set; }
        public bool SWATDuty { get; set; }
        public DateTime Created { get; set; }
        public int Level { get; set; }
        public int Xp { get; set; }
        public int SocialBonusMoney { get; set; }
        public int XpTicks { get; set; }
        public DateTime BannedUntil { get; set; }
        public string BanReason { get; set; }

        public bool Phone { get; set; }
        public bool Laptop { get; set; }
        public bool Backpack { get; set; }

        public int PhoneNumber { get; set; }
        public int PhoneBackground { get; set; }

        public int PositionId { get; set; }
        public int CustomizationId { get; set; }
        public int ClothesId { get; set; }
        public int InventoryId { get; set; }
        public int LaboratoryInputInventoryId { get; set; }
        public int LaboratoryOutputInventoryId { get; set; }
        public int LockerInventoryId { get; set; }
        public int BusinessId { get; set; }
        public int LicenseId { get; set; }

        public int TeamId { get; set; }
        public int TeamRank { get; set; }
        public bool TeamAdmin { get; set; }
        public bool TeamBank { get; set; }
        public bool TeamStorage { get; set; }
        public DateTime TeamJoinDate { get; set; }
        public bool TeamDuty { get; set; }

        public string FederalRecordTeam { get; set; }
        public string FederalRecordDescription { get; set; }
        public string FederalRecordPhone { get; set; }

        public int Jailtime { get; set; }

        public bool IsInHospital { get; set; }

        public AccountModel()
        {
            Name = string.Empty;
            FederalRecordTeam = string.Empty;
            FederalRecordDescription = string.Empty;
            FederalRecordPhone = string.Empty;
        }

        public AccountModel(string name, ulong socialclubId, ulong hardwareId, ulong hardwareIdEx, long discordId, bool whitelisted, int money, int bankMoney, ushort health, ushort armor, InjuryType injuryType, bool alive, bool coma, bool stabilized, AdminRank adminRank, DateTime lastOnline, int hunger, int thirst, bool cuffed, bool roped, bool sWATDuty, bool phone, bool laptop, bool backpack, int phoneNumber, int phoneBackground, int positionId, int customizationId, int clothesId, int inventoryId, int laboratoryInputInventoryId, int laboratoryOutputInventoryId, int lockerInventoryId, int businessId, int teamId, int teamRank, bool teamAdmin, bool teamBank, bool teamStorage, DateTime teamJoinDate, bool teamDuty, string federalRecordTeam, string federalRecordDescription, string federalRecordPhone, int jailtime)
        {
            Name = name;
            SocialclubId = socialclubId;
            HardwareId = hardwareId;
            HardwareIdEx = hardwareIdEx;
            DiscordId = discordId;
            Whitelisted = whitelisted;
            Money = money;
            BankMoney = bankMoney;
            Health = health;
            Armor = armor;
            InjuryType = injuryType;
            Alive = alive;
            Coma = coma;
            Stabilized = stabilized;
            AdminRank = adminRank;
            LastOnline = lastOnline;
            Hunger = hunger;
            Thirst = thirst;
            Cuffed = cuffed;
            Roped = roped;
            SWATDuty = sWATDuty;
            Phone = phone;
            Laptop = laptop;
            Backpack = backpack;
            PhoneNumber = phoneNumber;
            PhoneBackground = phoneBackground;
            PositionId = positionId;
            CustomizationId = customizationId;
            ClothesId = clothesId;
            InventoryId = inventoryId;
            LaboratoryInputInventoryId = laboratoryInputInventoryId;
            LaboratoryOutputInventoryId = laboratoryOutputInventoryId;
            LockerInventoryId = lockerInventoryId;
            BusinessId = businessId;
            TeamId = teamId;
            TeamRank = teamRank;
            TeamAdmin = teamAdmin;
            TeamBank = teamBank;
            TeamStorage = teamStorage;
            TeamJoinDate = teamJoinDate;
            TeamDuty = teamDuty;
            FederalRecordTeam = federalRecordTeam;
            FederalRecordDescription = federalRecordDescription;
            FederalRecordPhone = federalRecordPhone;
            Jailtime = jailtime;
            Level = 1;
        }
    }
}