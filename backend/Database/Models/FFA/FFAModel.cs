namespace Database.Models.FFA
{
	public class FFAModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int MaxPlayers { get; set; }

		public FFAModel()
		{
			Name = string.Empty;
		}

		public FFAModel(string name, int maxPlayers)
		{
			Name = name;
			MaxPlayers = maxPlayers;
		}
	}
}