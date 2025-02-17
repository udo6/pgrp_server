﻿using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.ClothesShop
{
    public class ClothesShopModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public ClothesShopType Type { get; set; }

        public ClothesShopModel()
        {
            Name = string.Empty;
        }

        public ClothesShopModel(string name, int positionId, ClothesShopType type)
        {
            Name = name;
            PositionId = positionId;
            Type = type;
        }
    }

	public class ClothesShopModelConfiguration : IEntityTypeConfiguration<ClothesShopModel>
	{
		public void Configure(EntityTypeBuilder<ClothesShopModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_clothesshops");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}