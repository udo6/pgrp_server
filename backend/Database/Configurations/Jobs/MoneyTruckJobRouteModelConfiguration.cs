using Database.Models.GarbageJob;
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
    public class MoneyTruckJobRouteModelConfiguration : IEntityTypeConfiguration<MoneyTruckJobRouteModel>
    {
        public void Configure(EntityTypeBuilder<MoneyTruckJobRouteModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("server_money_truck_job_routes");
            builder.HasIndex(x => x.Id).HasDatabaseName("id");
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
            builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(50)");
            builder.Property(x => x.Reward).HasColumnName("reward").HasColumnType("int(11)");
        }
    }
}
