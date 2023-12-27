namespace Database.Models.GardenerJob;

public class GardenerJobModel
{
    public int Id { get; set; }
    public int PositionId { get; set; }
    public int VehicleSpawnPositionId { get; set; }

    public GardenerJobModel() {}
}