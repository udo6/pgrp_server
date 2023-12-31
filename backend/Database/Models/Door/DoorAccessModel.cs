using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Door
{
	public class DoorAccessModel
	{
		public int Id { get; set; }
		public int DoorId { get; set; }
		public int OwnerId { get; set; }
		public OwnerType OwnerType { get; set; }

		public DoorAccessModel()
		{
		}

		public DoorAccessModel(int doorId, int ownerId, OwnerType ownerType)
		{
			Id = doorId;
			OwnerId = ownerId;
			OwnerType = ownerType;
		}
	}

	public class DoorAccessModelConfiguration : IEntityTypeConfiguration<DoorAccessModel>
	{
		public void Configure(EntityTypeBuilder<DoorAccessModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_door_access");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.DoorId).HasColumnName("door_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerId).HasColumnName("owner_id").HasColumnType("int(11)");
			builder.Property(x => x.OwnerType).HasColumnName("owner_type").HasColumnType("int(11)");
		}
	}
}
