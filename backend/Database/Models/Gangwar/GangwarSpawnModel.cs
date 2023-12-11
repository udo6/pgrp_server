namespace Database.Models.Gangwar
{
    public class GangwarSpawnModel
    {
        public int Id { get; set; }
        public int GangwarId { get; set; }
        public int PositionId { get; set; }
        public bool Team { get; set; }

        public GangwarSpawnModel()
        {

        }

        public GangwarSpawnModel(int gangwarId, int positionId, bool team)
        {
            GangwarId = gangwarId;
            PositionId = positionId;
            Team = team;
        }
    }
}