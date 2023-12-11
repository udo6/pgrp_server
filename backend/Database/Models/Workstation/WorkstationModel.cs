namespace Database.Models.Workstation
{
    public class WorkstationModel
    {
        public int Id { get; set; }
        public int PositionId { get; set; }

        public WorkstationModel() { }

        public WorkstationModel(int positionId)
        {
            PositionId = positionId;
        }
    }
}