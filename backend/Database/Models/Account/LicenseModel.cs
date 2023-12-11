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
}