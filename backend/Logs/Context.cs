using Logs.Models;
using Logs.Models.Inventory;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace Logs
{
	public class Context : DbContext
	{
		public DbSet<InventoryMoveModel> InventoryMoveLogs { get; set; }
		public DbSet<MoneyTransactionModel> MoneyTransactionLogs { get; set; }
		public DbSet<PlayerConnectionModel> PlayerConnectionLogs { get; set; }
		public DbSet<BanModel> BanLogs { get; set; }
		public DbSet<ExplosionModel> Explosions { get; set; }
		public DbSet<ACPActionModel> ACPActions { get; set; }
		public DbSet<KillModel> KillLogs { get; set; }
		public DbSet<DamageModel> DamageLogs { get; set; }
		public DbSet<MagicBulletModel> MagicBulletLogs { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (optionsBuilder.IsConfigured) return;

			var connectionString = new MySqlConnectionStringBuilder
			{
				Server = "localhost",
				Port = 3306,
				UserID = "mason",
				Password = "3ad51Ox1ciso3uSaL53ARalo74SIYo",
				Database = "pgrp_logs",
			};

			optionsBuilder.UseMySql(connectionString.ConnectionString, ServerVersion.AutoDetect(connectionString.ConnectionString)).EnableSensitiveDataLogging();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
		}
	}
}