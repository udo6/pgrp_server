using AltV.Net.Elements.Entities;
using Core.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Farming
{
    public class FarmingSpotModel
    {
        public int Id { get; set; }
        public int FarmingId { get; set; }
        public int PositionId { get; set; }

        [NotMapped]
        public RPShape? Shape { get; set; }

        [NotMapped]
        public int Health { get; set; } = 100;

        [NotMapped]
        public DateTime Despawned { get; set; }

        [NotMapped]
        public IObject Object { get; set; }

        public FarmingSpotModel() { }

        public FarmingSpotModel(int farmingId, int positionId)
        {
            FarmingId = farmingId;
            PositionId = positionId;
        }
    }
}