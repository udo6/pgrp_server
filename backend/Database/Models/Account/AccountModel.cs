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
        public bool DamageCap { get; set; }
        public string SupportCallMessage { get; set; }

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

        public string AdminRecordDescription { get; set; }

        public int Jailtime { get; set; }

        public bool IsInHospital { get; set; }

        public AccountModel()
        {
            Name = string.Empty;
            BanReason = string.Empty;
            SupportCallMessage = string.Empty;
            FederalRecordTeam = string.Empty;
            FederalRecordDescription = string.Empty;
            FederalRecordPhone = string.Empty;
            BanReason = string.Empty;
            AdminRecordDescription = string.Empty;

		}

        public AccountModel(string name, ulong social, ulong hwid, ulong hwidEx, long discord, int money, int bankMoney, int phoneNumber, int positionId, int customId, int clothesId, int inventoryId, int labInputInventoryId, int labOutputInventoryId, int lockerInventoryId, int businessId, int licenseId)
        {
            Name = name;
            SocialclubId = social;
            HardwareId = hwid;
            HardwareIdEx = hwidEx;
            DiscordId = discord;
            Whitelisted = false;
            Money = money;
            BankMoney = bankMoney;
            Health = 200;
            Armor = 0;
            ArmorItemId = 0;
            InjuryType = 0;
            Alive = true;
            Coma = false;
            Stabilized = false;
            AdminRank = AdminRank.SPIELER;
            LastOnline = DateTime.Now;
            Hunger = 100;
            Thirst = 100;
            Cuffed = false;
            Roped = false;
            SWATDuty = false;
            Created = DateTime.Now;
            Level = 1;
            Xp = 0;
            SocialBonusMoney = 0;
            XpTicks = 0;
            BannedUntil = DateTime.Now.AddYears(-1);
            BanReason = string.Empty;
            DamageCap = false;
            SupportCallMessage = string.Empty;
            Phone = false;
            Laptop = false;
            Backpack = false;
            PhoneNumber = phoneNumber;
            PhoneBackground = 0;
            PositionId = positionId;
			CustomizationId = customId;
			ClothesId = clothesId;
			InventoryId = inventoryId;
			LaboratoryInputInventoryId = labInputInventoryId;
			LaboratoryOutputInventoryId = labOutputInventoryId;
			LockerInventoryId = lockerInventoryId;
			BusinessId = businessId;
			LicenseId = licenseId;
            TeamId = 0;
            TeamRank = 0;
            TeamAdmin = false;
            TeamBank = false;
            TeamStorage = false;
            TeamJoinDate = DateTime.Now;
            TeamDuty = false;
            FederalRecordTeam = string.Empty;
            FederalRecordPhone = string.Empty;
            FederalRecordDescription = string.Empty;
            AdminRecordDescription = string.Empty;
            Jailtime = 0;
            IsInHospital = false;
		}
	}
}