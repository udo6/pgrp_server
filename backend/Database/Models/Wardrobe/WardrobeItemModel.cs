﻿using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Wardrobe
{
    public class WardrobeItemModel
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public OwnerType OwnerType { get; set; }
        public string Label { get; set; }
        public int Gender { get; set; }
        public int Component { get; set; }
        public int Drawable { get; set; }
        public int Texture { get; set; }
        public uint Dlc { get; set; }
        public bool Prop { get; set; }

        public WardrobeItemModel()
        {
            Label = string.Empty;
        }

        public WardrobeItemModel(int ownerId, OwnerType ownerType, string label, int gender, int component, int drawable, int texture, uint dlc, bool prop)
        {
            OwnerId = ownerId;
            OwnerType = ownerType;
            Label = label;
            Gender = gender;
            Component = component;
            Drawable = drawable;
            Texture = texture;
            Dlc = dlc;
            Prop = prop;
        }
    }

	public class WardrobeItemModelConfiguration : IEntityTypeConfiguration<WardrobeItemModel>
	{
		public void Configure(EntityTypeBuilder<WardrobeItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_wardrobe_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.Gender).HasColumnName("gender").HasColumnType("int(11)");
			builder.Property(x => x.Component).HasColumnName("component").HasColumnType("int(11)");
			builder.Property(x => x.Drawable).HasColumnName("drawable").HasColumnType("int(11)");
			builder.Property(x => x.Texture).HasColumnName("texture").HasColumnType("int(11)");
			builder.Property(x => x.Dlc).HasColumnName("dlc").HasColumnType("int(11)");
			builder.Property(x => x.Prop).HasColumnName("prop").HasColumnType("tinyint(1)");
		}
	}
}