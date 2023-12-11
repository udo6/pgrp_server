using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Database.Models.Door;

namespace Database.Configurations.Door
{
	public class DoorModelConfiguration : IEntityTypeConfiguration<DoorModel>
	{
		public void Configure(EntityTypeBuilder<DoorModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_doors");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
			builder.Property(x => x.Locked).HasColumnName("locked").HasColumnType("tinyint(1)");
			builder.Property(x => x.Radius).HasColumnName("radius").HasColumnType("float");
		}
	}
}
