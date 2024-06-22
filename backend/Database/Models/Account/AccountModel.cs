using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Account
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string IP { get; set; }
        public ulong SocialclubId { get; set; }
        public ulong HardwareId { get; set; }
        public ulong HardwareIdEx { get; set; }
        public ulong DiscordId { get; set; }
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
        public DateTime TeamLeaveDate { get; set; }

        public string FederalRecordTeam { get; set; }
        public string FederalRecordDescription { get; set; }
        public string FederalRecordPhone { get; set; }

        public string AdminRecordDescription { get; set; }

        public int Jailtime { get; set; }

        public bool IsInHospital { get; set; }

        public float PhoneVolume { get; set; }
        public bool BanOnConnect { get; set; }

        public int TeamLockerId { get; set; }

        public AccountModel()
        {
            Name = string.Empty;
            IP = string.Empty;
            BanReason = string.Empty;
            SupportCallMessage = string.Empty;
            FederalRecordTeam = string.Empty;
            FederalRecordDescription = string.Empty;
            FederalRecordPhone = string.Empty;
            BanReason = string.Empty;
            AdminRecordDescription = string.Empty;

		}

        public AccountModel(string name, string ip, ulong social, ulong hwid, ulong hwidEx, ulong discord, int money, int bankMoney, int phoneNumber, int positionId, int customId, int clothesId, int inventoryId, int labInputInventoryId, int labOutputInventoryId, int lockerInventoryId, int businessId, int licenseId, bool banOnConnect, int teamLockerId)
        {
            Name = name;
            IP = ip;
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
            LastOnline = DateTime.Now.AddYears(-20);
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
            TeamLeaveDate = DateTime.Now.AddYears(-1);
            FederalRecordTeam = string.Empty;
            FederalRecordPhone = string.Empty;
            FederalRecordDescription = string.Empty;
            AdminRecordDescription = string.Empty;
            Jailtime = 0;
            IsInHospital = false;
            PhoneVolume = 0.1f;
            BanOnConnect = banOnConnect;
            TeamLockerId = teamLockerId;
		}
	}

	public class AccountModelConfiguration : IEntityTypeConfiguration<AccountModel>
	{
		public void Configure(EntityTypeBuilder<AccountModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_accounts");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.IP).HasColumnName("ip").HasColumnType("varchar(255)");
			builder.Property(x => x.SocialclubId).HasColumnName("social").HasColumnType("bigint(20)");
			builder.Property(x => x.HardwareId).HasColumnName("hwid").HasColumnType("bigint(20)");
			builder.Property(x => x.HardwareIdEx).HasColumnName("hwid_ex").HasColumnType("bigint(20)");
			builder.Property(x => x.DiscordId).HasColumnName("discord").HasColumnType("bigint(20)");
			builder.Property(x => x.Whitelisted).HasColumnName("whitelisted").HasColumnType("tinyint(1)");

			builder.Property(x => x.InventoryId).HasColumnName("inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.LaboratoryInputInventoryId).HasColumnName("lab_input_inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.LaboratoryOutputInventoryId).HasColumnName("lab_output_inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.InventoryId).HasColumnName("inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.LockerInventoryId).HasColumnName("locker_inventory_id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("pos_id").HasColumnType("int(11)");
			builder.Property(x => x.CustomizationId).HasColumnName("customization_id").HasColumnType("int(11)");
			builder.Property(x => x.ClothesId).HasColumnName("clothes_id").HasColumnType("int(11)");
			builder.Property(x => x.BusinessId).HasColumnName("business_id").HasColumnType("int(11)");
			builder.Property(x => x.LicenseId).HasColumnName("license_id").HasColumnType("int(11)");

			builder.Property(x => x.TeamId).HasColumnName("team_id").HasColumnType("int(11)");
			builder.Property(x => x.TeamRank).HasColumnName("team_rank").HasColumnType("int(11)");
			builder.Property(x => x.TeamAdmin).HasColumnName("team_leader").HasColumnType("tinyint(1)");
			builder.Property(x => x.TeamBank).HasColumnName("team_bank").HasColumnType("tinyint(1)");
			builder.Property(x => x.TeamStorage).HasColumnName("team_storage").HasColumnType("tinyint(1)");
			builder.Property(x => x.TeamJoinDate).HasColumnName("team_join").HasColumnType("datetime");
			builder.Property(x => x.TeamDuty).HasColumnName("team_duty").HasColumnType("tinyint(1)");
			builder.Property(x => x.TeamLeaveDate).HasColumnName("team_leave").HasColumnType("datetime");

			builder.Property(x => x.Phone).HasColumnName("phone").HasColumnType("tinyint(1)");
			builder.Property(x => x.Laptop).HasColumnName("laptop").HasColumnType("tinyint(1)");
			builder.Property(x => x.Backpack).HasColumnName("backpack").HasColumnType("tinyint(1)");

			builder.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasColumnType("int(11)");
			builder.Property(x => x.PhoneBackground).HasColumnName("phone_background").HasColumnType("int(11)");

			builder.Property(x => x.Money).HasColumnName("money").HasColumnType("int(11)");
			builder.Property(x => x.BankMoney).HasColumnName("bank_money").HasColumnType("int(11)");
			builder.Property(x => x.Health).HasColumnName("health").HasColumnType("int(11)");
			builder.Property(x => x.Armor).HasColumnName("armor").HasColumnType("int(11)");
			builder.Property(x => x.ArmorItemId).HasColumnName("armor_item_id").HasColumnType("int(11)");
			builder.Property(x => x.InjuryType).HasColumnName("injury_type").HasColumnType("int(11)");
			builder.Property(x => x.Alive).HasColumnName("alive").HasColumnType("tinyint(1)");
			builder.Property(x => x.Coma).HasColumnName("coma").HasColumnType("tinyint(1)");
			builder.Property(x => x.Stabilized).HasColumnName("stabilized").HasColumnType("tinyint(1)");
			builder.Property(x => x.AdminRank).HasColumnName("admin_rank").HasColumnType("int(11)");
			builder.Property(x => x.LastOnline).HasColumnName("last_online").HasColumnType("datetime");
			builder.Property(x => x.SWATDuty).HasColumnName("swat").HasColumnType("tinyint(1)");
			builder.Property(x => x.Created).HasColumnName("created").HasColumnType("datetime");
			builder.Property(x => x.Level).HasColumnName("level").HasColumnType("int(11)");
			builder.Property(x => x.Xp).HasColumnName("xp").HasColumnType("int(11)");
			builder.Property(x => x.XpTicks).HasColumnName("xp_ticks").HasColumnType("int(11)");
			builder.Property(x => x.SocialBonusMoney).HasColumnName("social_bonus_money").HasColumnType("int(11)");
			builder.Property(x => x.BannedUntil).HasColumnName("banned_until").HasColumnType("datetime");
			builder.Property(x => x.BanReason).HasColumnName("ban_reason").HasColumnType("varchar(255)");
			builder.Property(x => x.DamageCap).HasColumnName("damage_cap").HasColumnType("tinyint(1)");
			builder.Property(x => x.SupportCallMessage).HasColumnName("support_call_message").HasColumnType("varchar(255)");

			builder.Property(x => x.Hunger).HasColumnName("hunger").HasColumnType("int(11)");
			builder.Property(x => x.Thirst).HasColumnName("thirst").HasColumnType("int(11)");
			builder.Property(x => x.Cuffed).HasColumnName("cuffed").HasColumnType("tinyint(1)");
			builder.Property(x => x.Roped).HasColumnName("roped").HasColumnType("tinyint(1)");

			builder.Property(x => x.FederalRecordTeam).HasColumnName("federal_record_team").HasColumnType("varchar(255)");
			builder.Property(x => x.FederalRecordPhone).HasColumnName("federal_record_phone").HasColumnType("varchar(255)");
			builder.Property(x => x.FederalRecordDescription).HasColumnName("federal_record_description").HasColumnType("longtext");

			builder.Property(x => x.AdminRecordDescription).HasColumnName("admin_record").HasColumnType("longtext");

			builder.Property(x => x.Jailtime).HasColumnName("jailtime").HasColumnType("int(11)");

			builder.Property(x => x.IsInHospital).HasColumnName("hospital").HasColumnType("tinyint(1)");

			builder.Property(x => x.PhoneVolume).HasColumnName("phone_volume").HasColumnType("float");
			builder.Property(x => x.BanOnConnect).HasColumnName("ban_on_connect").HasColumnType("tinyint(1)");

			builder.Property(x => x.TeamLockerId).HasColumnName("team_locker_id").HasColumnType("int(11)");
		}
	}
}