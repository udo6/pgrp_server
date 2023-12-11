namespace Database.Models.VehicleShop
{
    public class VehicleShopSpawnModel
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int PositionId { get; set; }

        public VehicleShopSpawnModel()
        {
        }

        public VehicleShopSpawnModel(int shopId, int positionId)
        {
            ShopId = shopId;
            PositionId = positionId;
        }
    }
}