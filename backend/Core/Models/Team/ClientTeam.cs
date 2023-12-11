namespace Backend.Utils.Models.Team.Client
{
	public class ClientTeam
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int Warns { get; set; }
		public int Gws { get; set; }
		public int Money { get; set; }
		public TeamStorageClientData Storage { get; set; }
		public List<TeamClientMemberData> Members { get; set; }
		public List<object> BankHistory { get; set; }
		public TeamClientLaboratoryData Laboratory { get; set; }
		public TeamClientBusinessData Business { get; set; }

		public ClientTeam(int id, string name, int warns, int gws, int money, TeamStorageClientData storage, List<TeamClientMemberData> members, List<object> bankHistory, TeamClientLaboratoryData laboratory, TeamClientBusinessData business)
		{
			Id = id;
			Name = name;
			Warns = warns;
			Gws = gws;
			Money = money;
			Storage = storage;
			Members = members;
			BankHistory = bankHistory;
			Laboratory = laboratory;
			Business = business;
		}
	}
}