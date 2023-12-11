namespace Core.Models.Laptop
{
	public class Unit
	{
		private static int IdCounter = 1;

		public int Id { get; set; }
		public string Name { get; set; }
		public int Vehicle { get; set; }
		public List<UnitMember> Members { get; set; }
		public int Team { get; set; }

		public Unit()
		{
			Id = IdCounter++;
			Name = string.Empty;
			Members = new();
		}

		public Unit(string name, int vehicle, List<UnitMember> members, int team)
		{
			Id = IdCounter++;
			Name = name;
			Vehicle = vehicle;
			Members = members;
			Team = team;
		}
	}
}