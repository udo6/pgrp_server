using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Account
{
    public class LicenseModel
    {
        public int Id { get; set; }
        public bool Car { get; set; }
        public DateTime CarRevoked { get; set; }
        public bool Truck { get; set; }
        public DateTime TruckRevoked { get; set; }
        public bool Heli { get; set; }
        public DateTime HeliRevoked { get; set; }
        public bool Plane { get; set; }
        public DateTime PlaneRevoked { get; set; }
        public bool Boat { get; set; }
        public DateTime BoatRevoked { get; set; }
        public bool Taxi { get; set; }
        public DateTime TaxiRevoked { get; set; }
        public bool Lawyer { get; set; }
        public DateTime LawyerRevoked { get; set; }
        public bool Gun { get; set; }
        public DateTime GunRevoked { get; set; }

        public LicenseModel()
        {
        }

        public LicenseModel(bool car, bool truck, bool heli, bool plane, bool boat, bool taxi, bool lawyer, bool gun)
        {
            Car = car;
            Truck = truck;
            Heli = heli;
            Plane = plane;
            Boat = boat;
            Taxi = taxi;
            Lawyer = lawyer;
            Gun = gun;
        }
    }

	public class LicenseModelConfiguration : IEntityTypeConfiguration<LicenseModel>
	{
		public void Configure(EntityTypeBuilder<LicenseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_licenses");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Car).HasColumnName("car").HasColumnType("tinyint(1)");
			builder.Property(x => x.CarRevoked).HasColumnName("car_revoked").HasColumnType("datetime");
			builder.Property(x => x.Truck).HasColumnName("truck").HasColumnType("tinyint(1)");
			builder.Property(x => x.TruckRevoked).HasColumnName("truck_revoked").HasColumnType("datetime");
			builder.Property(x => x.Heli).HasColumnName("heli").HasColumnType("tinyint(1)");
			builder.Property(x => x.HeliRevoked).HasColumnName("heli_revoked").HasColumnType("datetime");
			builder.Property(x => x.Plane).HasColumnName("plane").HasColumnType("tinyint(1)");
			builder.Property(x => x.PlaneRevoked).HasColumnName("plane_revoked").HasColumnType("datetime");
			builder.Property(x => x.Boat).HasColumnName("boat").HasColumnType("tinyint(1)");
			builder.Property(x => x.BoatRevoked).HasColumnName("boat_revoked").HasColumnType("datetime");
			builder.Property(x => x.Taxi).HasColumnName("taxi").HasColumnType("tinyint(1)");
			builder.Property(x => x.TaxiRevoked).HasColumnName("taxi_revoked").HasColumnType("datetime");
			builder.Property(x => x.Lawyer).HasColumnName("lawyer").HasColumnType("tinyint(1)");
			builder.Property(x => x.LawyerRevoked).HasColumnName("lawyer_revoked").HasColumnType("datetime");
			builder.Property(x => x.Gun).HasColumnName("gun").HasColumnType("tinyint(1)");
			builder.Property(x => x.GunRevoked).HasColumnName("gun_revoked").HasColumnType("datetime");
		}
	}
}