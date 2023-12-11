namespace Database.Models.Gangwar
{
    public class GangwarFlagModel
    {
        public int Id { get; set; }
        public int GangwarId { get; set; }
        public int PositionId { get; set; }

        public GangwarFlagModel() { }

        public GangwarFlagModel(int gangwarId, int positionId)
        {
            GangwarId = gangwarId;
            PositionId = positionId;
        }
    }
}