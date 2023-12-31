using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.MoneyTruckJob
{
    public class MoneyTruckJobRoutePositionModel
    {
        public int Id { get; set; }
        public int RouteId { get; set; }
        public int PositionId { get; set; }

        public MoneyTruckJobRoutePositionModel() { }
        public MoneyTruckJobRoutePositionModel(int routeId, int positionId) 
        {
            RouteId = routeId;
            PositionId = positionId;
        }
    }

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
