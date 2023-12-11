﻿using Database.Models.Door;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Configurations.Door
{
	public class DoorEntityModelConfiguration : IEntityTypeConfiguration<DoorEntityModel>
	{
		public void Configure(EntityTypeBuilder<DoorEntityModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_door_entities");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.DoorId).HasColumnName("door_id").HasColumnType("int(11)");
			builder.Property(x => x.Model).HasColumnName("model").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}
