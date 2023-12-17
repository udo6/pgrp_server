using Core.Enums;

namespace Database.Models.Workstation
{
    public class WorkstationModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }
        public WorkstationType Type { get; set; }
        public int MaxActiveItems { get; set; }
        public uint PedModel { get; set; }

        public WorkstationModel() { }

        public WorkstationModel(int positionId, WorkstationType type, int maxActiveItems, uint pedModel)
        {
            PositionId = positionId;
            Type = type;
            MaxActiveItems = maxActiveItems;
            PedModel = pedModel;
        }
    }
}