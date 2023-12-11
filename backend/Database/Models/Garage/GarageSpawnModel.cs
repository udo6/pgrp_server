namespace Database.Models.Garage
{
    public class GarageSpawnModel
    {
        public int Id { get; set; }
        public int GarageId { get; set; }
        public int PositionId { get; set; }

        public GarageSpawnModel()
        {

        }

        public GarageSpawnModel(int garageId, int positionId)
        {
            GarageId = garageId;
            PositionId = positionId;
        }
    }
}