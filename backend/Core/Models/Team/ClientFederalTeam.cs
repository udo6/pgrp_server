namespace Backend.Utils.Models.Team.Client
{
	public class ClientFederalTeam
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Warns { get; set; }
		public bool SWAT { get; set; }
		public int Money { get; set; }
		public List<TeamClientMemberData> Members { get; set; }
		public List<object> BankHistory { get; set; }
		public TeamClientBusinessData Business { get; set; }

		public ClientFederalTeam(int id, string name, int warns, bool sWAT, int money, List<TeamClientMemberData> members, List<object> bankHistory, TeamClientBusinessData business)
		{
			Id = id;
			Name = name;
			Warns = warns;
			SWAT = sWAT;
			Money = money;
			Members = members;
			BankHistory = bankHistory;
			Business = business;
		}
	}
}