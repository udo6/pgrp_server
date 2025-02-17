﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Tuner
{
	public class TunerModel
	{
		public int Id { get; set; }
		public int PositionId { get; set; }

		public TunerModel()
		{
		}

		public TunerModel(int positionId)
		{
			PositionId = positionId;
		}
	}

	public class TunerModelConfiguration : IEntityTypeConfiguration<TunerModel>
	{
		public void Configure(EntityTypeBuilder<TunerModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_tuner");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}
