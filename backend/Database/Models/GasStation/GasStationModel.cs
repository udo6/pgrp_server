namespace Database.Models.GasStation
{
    public class GasStationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public int Price { get; set; }
        public int MinPrice { get; set; }
        public int MaxPrice { get; set; }

        public GasStationModel()
        {
            Name = string.Empty;
        }

        public GasStationModel(string name, int positionId, int minPrice, int maxPrice)
        {
            Name = name;
            PositionId = positionId;
            Price = 0;
            MinPrice = minPrice;
            MaxPrice = maxPrice;
        }
    }
}