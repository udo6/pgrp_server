namespace Core.Models.Laptop
{
	public class UnitMember
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Rank { get; set; }

		public UnitMember()
		{
			Name = string.Empty;
		}

		public UnitMember(int id, string name, int rank)
		{
			Id = id;
			Name = name;
			Rank = rank;
		}
	}
}