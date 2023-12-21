using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Database.Models.Account;

namespace Database.Configurations
{
    public class AccountModelConfiguration : IEntityTypeConfiguration<AccountModel>
	{
		public void Configure(EntityTypeBuilder<AccountModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_accounts");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
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
		}
	}
}