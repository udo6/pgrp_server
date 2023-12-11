using AltV.Net.Elements.Entities;

namespace Core.Models.Gangwar
{
	public class RunningGangwar
	{
		public int DbId { get; set; }
		public string Name { get; set; }
		public int OwnerId { get; set; }
		public string OwnerName { get; set; }
		public int OwnerPoints { get; set; }
		public int AttackerId { get; set; }
		public string AttackerName { get; set; }
		public int AttackerPoints { get; set; }

		public DateTime Started { get; set; }

		// markers
		public List<IMarker> Markers { get; set; }

		public RunningGangwar()
		{
			Name = string.Empty;
			OwnerName = string.Empty;
			AttackerName = string.Empty;
			Markers = new();

			Started = DateTime.Now;
		}

		public RunningGangwar(int dbId, string name, int ownerId, string ownerName, int ownerPoints, int attackerId, string attackerName, int attackerPoints, params IMarker[] markers)
		{
			DbId = dbId;
			Name = name;
			OwnerId = ownerId;
			OwnerName = ownerName;
			OwnerPoints = ownerPoints;
			AttackerId = attackerId;
			AttackerName = attackerName;
			AttackerPoints = attackerPoints;
			Markers = new(markers);

			Started = DateTime.Now;
		}
	}
}