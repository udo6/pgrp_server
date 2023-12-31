using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.MoneyTruckJob
{
    public class MoneyTruckJobRouteModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Reward { get; set; }

        public MoneyTruckJobRouteModel()
		{
			Name = string.Empty;
		}
        public MoneyTruckJobRouteModel(string name, int reward)
        {
            Name = name;
            Reward = reward;
        }
    }

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
