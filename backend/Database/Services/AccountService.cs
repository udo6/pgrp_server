using Database.Models.Account;
using Database.Models.Bank;

namespace Database.Services
{
    public static class AccountService
	{
		public static int GenerateUniquePhoneNumber()
		{
			using var ctx = new Context();

			var number = 0;
			var random = new Random();

			while(true)
			{
				var tempNumber = random.Next(10000000, 99999999);
				if (ctx.Accounts.Any(x => x.PhoneNumber == tempNumber)) continue;

				number = tempNumber;
				break;
			}

			return number;
		}

		public static List<AccountModel> GetAll()
		{
			using var ctx = new Context();
			return ctx.Accounts.ToList();
		}

		public static List<AccountModel> Search(string search, int take)
		{
			using var ctx = new Context();
			return ctx.Accounts.Where(x =>
				x.Id.ToString().ToLower().Contains(search.ToLower()) ||
				x.Name.ToLower().Contains(search.ToLower()) ||
				x.TeamId.ToString().ToLower().Contains(search.ToLower()) ||
				x.PhoneNumber.ToString().ToLower().Contains(search.ToLower())).Take(take).ToList();
		}

		public static List<int> GetAllIdOnly()
		{
			using var ctx = new Context();
			return ctx.Accounts.Select(x => x.Id).ToList();
		}

		public static List<AccountModel> GetFromName(string name, int max)
		{
			using var ctx = new Context();
			return ctx.Accounts.Where(x => x.Name.ToLower().Contains(name.ToLower())).Take(max).ToList();
		}

		public static AccountModel? Get(string name)
		{
			using var ctx = new Context();
			return ctx.Accounts.FirstOrDefault(x => x.Name.ToLower() == name.ToLower());
		}

		public static AccountModel? Get(ulong social, ulong hwid, ulong hwidex, long discord)
		{
			using var ctx = new Context();
			return ctx.Accounts.FirstOrDefault(x => x.SocialclubId == social || x.HardwareId == hwid || x.HardwareIdEx == hwidex || x.DiscordId == discord);
		}

		public static List<AccountModel> GetFromTeam(int teamId)
		{
			using var ctx = new Context();
			return ctx.Accounts.Where(x => x.TeamId == teamId).ToList();
		}

		public static bool IsNameTaken(string name)
		{
			using var ctx = new Context();
			return ctx.Accounts.Any(x => x.Name.ToLower() == name.ToLower());
		}

		public static void Add(AccountModel model)
		{
			using var ctx = new Context();
			ctx.Accounts.Add(model);
			ctx.SaveChanges();
		}

		public static void Remove(AccountModel model)
		{
			using var ctx = new Context();
			ctx.Accounts.Remove(model);
			ctx.SaveChanges();
		}

		public static AccountModel? Get(int id)
		{
			using var ctx = new Context();
			return ctx.Accounts.FirstOrDefault(x => x.Id == id);
		}

		public static void Update(AccountModel model)
		{
			using var ctx = new Context();
			ctx.Accounts.Update(model);
			ctx.SaveChanges();
		}

		public static void Update(IEnumerable<AccountModel> models)
		{
			using var ctx = new Context();
			ctx.Accounts.UpdateRange(models);
			ctx.SaveChanges();
		}

		// admin history

		public static List<AdminHistoryModel> GetPlayerAdminHistory(int accountId)
		{
			using var ctx = new Context();
			return ctx.AdminHistory.Where(x => x.AccountId == accountId).ToList();
		}

		public static void AddAdminHistory(AdminHistoryModel model)
		{
			using var ctx = new Context();
			ctx.AdminHistory.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveAdminHistory(AdminHistoryModel model)
		{
			using var ctx = new Context();
			ctx.AdminHistory.Remove(model);
			ctx.SaveChanges();
		}

		public static AdminHistoryModel? GetAdminHistory(int id)
		{
			using var ctx = new Context();
			return ctx.AdminHistory.FirstOrDefault(x => x.Id == id);
		}

		public static void UpdateAdminHistory(AdminHistoryModel model)
		{
			using var ctx = new Context();
			ctx.AdminHistory.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateAdminHistory(IEnumerable<AdminHistoryModel> models)
		{
			using var ctx = new Context();
			ctx.AdminHistory.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}