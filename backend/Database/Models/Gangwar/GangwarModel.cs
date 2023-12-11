using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Gangwar
{
    public class GangwarModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PositionId { get; set; }
        public int OwnerId { get; set; }
        public DateTime LastAttack { get; set; }
        public int AttackerSpawnPositionId { get; set; }
        public int DefenderSpawnPositionId { get; set; }

        public GangwarModel()
        {
            Name = string.Empty;
        }

        public GangwarModel(string name, int positionId, int ownerId, DateTime lastAttack, int attackerSpawnPositionId, int defenderSpawnPositionId)
        {
            Name = name;
            PositionId = positionId;
            OwnerId = ownerId;
            LastAttack = lastAttack;
            AttackerSpawnPositionId = attackerSpawnPositionId;
            DefenderSpawnPositionId = defenderSpawnPositionId;
        }
    }
}