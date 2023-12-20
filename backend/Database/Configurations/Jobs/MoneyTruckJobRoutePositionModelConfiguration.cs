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
    public class MoneyTruckJobRoutePositionModelConfiguration : IEntityTypeConfiguration<MoneyTruckJobRoutePositionModel>
    {
        public void Configure(EntityTypeBuilder<MoneyTruckJobRoutePositionModel> builder)
        {
            builder.HasKey(x => x.Id);
            builder.ToTable("server_money_truck_job_route_positions");
            builder.HasIndex(x => x.Id).HasDatabaseName("id");
            builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
            builder.Property(x => x.RouteId).HasColumnName("route_id").HasColumnType("int(11)");
            builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
        }
    }
}
