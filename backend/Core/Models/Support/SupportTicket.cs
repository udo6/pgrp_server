namespace Core.Models.Support
{
	public class SupportTicket
	{
		private static int IdCounter = 1;

		public int Id { get; set; }
		public int CreatorId { get; set; }
		public string Creator {  get; set; }
		public string Message { get; set; }
		public int AdminId { get; set; }
		public string Admin { get; set; }
		public string Date { get; set; }

		public SupportTicket()
		{
			Id = IdCounter++;
			Creator = string.Empty;
			Message = string.Empty;
			Admin = string.Empty;
			Date = string.Empty;
		}

		public SupportTicket(int creatorId, string creator, string message)
		{
			Id = IdCounter++;
			Creator = creator;
			CreatorId = creatorId;
			Message = message;
			AdminId = 0;
			Admin = string.Empty;
			Date = DateTime.Now.ToString("HH:mm");
		}
	}
}
