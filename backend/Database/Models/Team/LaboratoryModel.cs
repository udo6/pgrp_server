using Core.Enums;

namespace Database.Models.Team
{
    public class LaboratoryModel
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public int PositionId { get; set; }
        public int FuelInventoryId { get; set; }
        public bool Robbed { get; set; }
        public int RobInventoryId { get; set; }
        public LaboratoryType Type { get; set; }
        public DateTime LastAttack { get; set; }

        public LaboratoryModel()
        {

        }

        public LaboratoryModel(int teamId, int positionId, int fuelInventoryId, bool robbed, int robInventoryId, LaboratoryType type)
        {
            TeamId = teamId;
            PositionId = positionId;
            FuelInventoryId = fuelInventoryId;
            Robbed = robbed;
            RobInventoryId = robInventoryId;
            Type = type;
            LastAttack = DateTime.Now.AddDays(-3);
        }
    }
}