using AltV.Net.Data;

namespace Core.Models.Laptop
{
	public class Dispatch
	{
		private static int IdCounter = 1;

		public int Id { get; set; }
		public int CreatorId { get; set; }
		public string Creator { get; set; }
		public string Message { get; set; }
		public Position Position { get; set; }
		public string Officer { get; set; }
		public string Date { get; set; }
		public int Team { get; set; }

		public Dispatch()
		{
			Creator = string.Empty;
			Message = string.Empty;
			Officer = string.Empty;
			Date = string.Empty;
		}

		public Dispatch(int creatorId, string creator, string message, Position pos, string officer, string date, int team)
		{
			Id = IdCounter++;
			CreatorId = creatorId;
			Creator = creator;
			Message = message;
			Position = pos;
			Officer = officer;
			Date = date;
			Team = team;
		}
	}
}