﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Gangwar
{
    public class GangwarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public int OwnerId { get; set; }
        public DateTime LastAttack { get; set; }
        public int AttackerSpawnPositionId { get; set; }
        public int DefenderSpawnPositionId { get; set; }

        public GangwarModel()
        {
            Name = string.Empty;
        }

        public GangwarModel(string name, int positionId, int ownerId, DateTime lastAttack, int attackerSpawnPositionId, int defenderSpawnPositionId)
        {
            Name = name;
            PositionId = positionId;
            OwnerId = ownerId;
            LastAttack = lastAttack;
            AttackerSpawnPositionId = attackerSpawnPositionId;
            DefenderSpawnPositionId = defenderSpawnPositionId;
        }
    }

	public class GangwarModelConfiguration : IEntityTypeConfiguration<GangwarModel>
	{
		public void Configure(EntityTypeBuilder<GangwarModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_gangwars");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.LastAttack).HasColumnName("last_attack").HasColumnType("datetime");
			builder.Property(x => x.DefenderSpawnPositionId).HasColumnName("defender_spawn_position_id").HasColumnType("int(11)");
			builder.Property(x => x.AttackerSpawnPositionId).HasColumnName("attacker_spawn_position_id").HasColumnType("int(11)");
		}
	}
}