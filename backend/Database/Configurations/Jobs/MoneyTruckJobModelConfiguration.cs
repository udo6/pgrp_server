using Database.Models.MoneyTruckJob;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Configurations.Jobs
{
    public class MoneyTruckJobModelConfiguration : IEntityTypeConfiguration<MoneyTruckJobModel>
    {
        public void Configure(EntityTypeBuilder<MoneyTruckJobModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("server_money_truck_job");
            builder.HasIndex(x => x.Id).HasDatabaseName("id");
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
            builder.Property(x => x.StartLocationId).HasColumnName("start_location_id").HasColumnType("int(11)");
            builder.Property(x => x.SpawnLocationId).HasColumnName("spawn_location_id").HasColumnType("int(11)");
        }
    }
}
