using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Database.Models;
using Database.Models.Account;
using Database.Models.Vehicle;
using Database.Models.Gangwar;
using Database.Models.Bank;
using Database.Models.Animation;
using Database.Models.Wardrobe;
using Database.Models.Inventory;
using Database.Models.Farming;
using Database.Models.Crimes;
using Database.Models.Phone;
using Database.Models.VehicleShop;
using Database.Models.ClothesShop;
using Database.Models.Shop;
using Database.Models.Team;
using Database.Models.Warehouse;
using Database.Models.Garage;
using Database.Models.Lootdrop;
using Database.Models.Processor;
using Database.Models.GasStation;
using Database.Models.House;
using Database.Models.Jumpoint;
using Database.Models.Workstation;
using Database.Models.Barber;
using Database.Models.Tattoo;
using Database.Models.Hospital;
using Database.Models.DPOS;
using Database.Models.FFA;
using Database.Models.Dealer;
using Database.Models.Door;
using Database.Models.Tuner;
using Database.Models.GarbageJob;
using Database.Models.MoneyTruckJob;
using Database.Models.Case;
using Database.Models.GardenerJob;

namespace Database
{
    public class Context : DbContext
	{
		public DbSet<AccountModel> Accounts { get; set; }
		public DbSet<PositionModel> Positions { get; set; }
		public DbSet<CustomizationModel> Customizations { get; set; }
		public DbSet<InventoryModel> Inventories { get; set; }
		public DbSet<InventoryItemModel> InventoryItems { get; set; }
		public DbSet<ItemModel> Items { get; set; }
		public DbSet<LoadoutModel> Loadouts { get; set; }
		public DbSet<LoadoutAttatchmentModel> LoadoutAttatchments { get; set; }
		public DbSet<VehicleModel> Vehicles { get; set; }
		public DbSet<VehicleBaseModel> VehicleBase { get; set; }
		public DbSet<TuningModel> Tunings { get; set; }
		public DbSet<TeamModel> Teams { get; set; }
		public DbSet<LaboratoryModel> Laboratories { get; set; }
		public DbSet<BankModel> Banks { get; set; }
		public DbSet<BankHistoryModel> BankHistory { get; set; }
		public DbSet<ClothesModel> Clothes { get; set; }
		public DbSet<GangwarModel> Gangwars { get; set; }
		public DbSet<GangwarFlagModel> GangwarFlags { get; set; }
		public DbSet<AnimationModel> Animations { get; set; }
		public DbSet<AnimationCategoryModel> AnimationCategories { get; set; }
		public DbSet<AnimationFavoriteModel> AnimationFavorites { get; set; }
		public DbSet<GarageModel> Garages { get; set; }
		public DbSet<GarageSpawnModel> GarageSpawns { get; set; }
		public DbSet<JumppointModel> Jumppoints { get; set; }
		public DbSet<GangwarSpawnModel> GangwarSpawns { get; set; }
		public DbSet<ClothesShopModel> ClothesShops { get; set; }
		public DbSet<ClothesShopItemModel> ClothesShopItems { get; set; }
		public DbSet<BlipModel> Blips { get; set; }
		public DbSet<ShopModel> Shops { get; set; }
		public DbSet<ShopItemModel> ShopItems { get; set; }
		public DbSet<GasStationModel> GasStations { get; set; }
		public DbSet<CrimeBaseModel> CrimeBases { get; set; }
		public DbSet<CrimeGroupModel> CrimeGroups { get; set; }
		public DbSet<CrimeModel> Crimes { get; set; }
		public DbSet<PhoneContactModel> PhoneContacts { get; set; }
		public DbSet<PhoneChatModel> PhoneChats { get; set; }
		public DbSet<PhoneChatMessageModel> PhoneChatMessages { get; set; }
		public DbSet<HouseModel> Houses { get; set; }
		public DbSet<WarehouseModel> Warehouses { get; set; }
		public DbSet<WarehouseInventoryModel> WarehouseInventories { get; set; }
		public DbSet<LicenseModel> Licenses { get; set; }
		public DbSet<WardrobeModel> Wardrobes { get; set; }
		public DbSet<WardrobeItemModel> WardrobeItems { get; set; }
		public DbSet<OutfitModel> Outfits { get; set; }
		public DbSet<LootdropModel> Lootdrops { get; set; }
		public DbSet<LootdropLootModel> LootdropLoot { get; set; }
		public DbSet<VehicleShopModel> VehicleShops { get; set; }
		public DbSet<VehicleShopItemModel> VehicleShopItems { get; set; }
		public DbSet<VehicleShopSpawnModel> VehicleShopSpawns { get; set; }
		public DbSet<FarmingModel> Farmings { get; set; }
		public DbSet<FarmingSpotModel> FarmingSpots { get; set; }
		public DbSet<ProcessorModel> Processors { get; set; }
		public DbSet<WorkstationModel> Workstations { get; set; }
		public DbSet<WorkstationBlueprintModel> WorkstationBlueprints { get; set; }
		public DbSet<WorkstationItemModel> WorkstationItems { get; set; }
		public DbSet<ExportDealerItemModel> ExportDealerItems { get; set; }
		public DbSet<BarberModel> Barbers { get; set; }
		public DbSet<BarberStyleModel> BarberStyles { get; set; }
		public DbSet<BarberColorModel> BarberColors { get; set; }
		public DbSet<TattooModel> Tattoos { get; set; }
		public DbSet<TattooShopModel> TattooShops { get; set; }
		public DbSet<TattooShopItemModel> TattooShopItems { get; set; }
		public DbSet<HospitalModel> Hospitals { get; set; }
		public DbSet<HospitalBedModel> HospitalBeds { get; set; }
		public DbSet<InventoryItemAttributeModel> InventoryItemAttributes { get; set; }
		public DbSet<ImpoundModel> Impounds { get; set; }
		public DbSet<ImpoundSpawnModel> ImpoundSpawns { get; set; }
		public DbSet<FFAModel> FFAs { get; set; }
		public DbSet<FFASpawnModel> FFASpawns { get; set; }
		public DbSet<FFAWeaponModel> FFAWeapons { get; set; }
		public DbSet<DealerModel> Dealers { get; set; }
		public DbSet<DealerItemModel> DealerItems { get; set; }
		public DbSet<DoorModel> Doors { get; set; }
		public DbSet<DoorEntityModel> DoorEntities { get; set; }
		public DbSet<DoorAccessModel> DoorAccesses { get; set; }
		public DbSet<TunerModel> Tuner { get; set; }
		public DbSet<TunerCategoryModel> TunerCategories { get; set; }
		public DbSet<TunerItemModel> TunerItems { get; set; }
        public DbSet<GarbageJobModel> GarbageJobs { get; set; }
        public DbSet<WarnModel> Warns { get; set; }
        public DbSet<AdminHistoryModel> AdminHistory { get; set; }
        public DbSet<MoneyTruckJobModel> MoneyTruckJobs { get; set; }
        public DbSet<MoneyTruckJobRouteModel> MoneyTruckJobRoutes { get; set; }
        public DbSet<MoneyTruckJobRoutePositionModel> MoneyTruckJobRoutePosition { get; set; }
		public DbSet<CaseLootModel> CaseLootTable { get; set; }
		public DbSet<BarberBeardModel> BarberBeardModels { get; set; }
		public DbSet<GardenerJobModel> GardenerJobs { get; set; }
		public DbSet<ReportModel> Reports { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (optionsBuilder.IsConfigured) return;

			var connectionString = new MySqlConnectionStringBuilder
			{
				Server = "176.96.137.221",
				Port = 3306,
				UserID = "mason",
				Password = "Paiyai43234!",
				Database = "pgrp",
			};

			optionsBuilder.UseMySql(connectionString.ConnectionString, ServerVersion.AutoDetect(connectionString.ConnectionString)).EnableSensitiveDataLogging();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
		}
	}
}