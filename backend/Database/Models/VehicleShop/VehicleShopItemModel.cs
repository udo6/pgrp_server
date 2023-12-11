namespace Database.Models.VehicleShop
{
    public class VehicleShopItemModel
    {
        public int Id { get; set; }
        public int ShopId { get; set; }
        public int VehicleBaseId { get; set; }
        public int PositionId { get; set; }

        public VehicleShopItemModel() { }

        public VehicleShopItemModel(int shopId, int vehicleBaseId, int positionId)
        {
            ShopId = shopId;
            VehicleBaseId = vehicleBaseId;
            PositionId = positionId;
        }
    }
}