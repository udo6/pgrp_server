namespace Database.Models.VehicleShop
{
    public class VehicleShopModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public int PedPositionId { get; set; }

        public VehicleShopModel()
        {
        }

        public VehicleShopModel(int positionId, int pedPositionId)
        {
            PositionId = positionId;
            PedPositionId = pedPositionId;
        }
    }
}