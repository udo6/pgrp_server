using Database.Models.Phone;

namespace Database.Services
{
    public static class PhoneService
	{
		public static void AddContact(PhoneContactModel model)
		{
			using var ctx = new Context();
			ctx.PhoneContacts.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveContact(PhoneContactModel model)
		{
			using var ctx = new Context();
			ctx.PhoneContacts.Remove(model);
			ctx.SaveChanges();
		}

		public static PhoneContactModel? GetContact(int id)
		{
			using var ctx = new Context();
			return ctx.PhoneContacts.FirstOrDefault(x => x.Id == id);
		}

		public static bool HasContact(int accountId, int number)
		{
			using var ctx = new Context();
			return ctx.PhoneContacts.Any(x => x.AccountId == accountId && x.Number == number);
		}

		public static PhoneContactModel? GetContactByNumber(int accountId, int number)
		{
			using var ctx = new Context();
			return ctx.PhoneContacts.FirstOrDefault(x => x.AccountId == accountId && x.Number == number);
		}

		public static List<PhoneContactModel> GetContacts(int accountId)
		{
			using var ctx = new Context();
			return ctx.PhoneContacts.Where(x => x.AccountId == accountId).ToList();
		}

		public static void UpdateContact(PhoneContactModel model)
		{
			using var ctx = new Context();
			ctx.PhoneContacts.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateContacts(IEnumerable<PhoneContactModel> models)
		{
			using var ctx = new Context();
			ctx.PhoneContacts.UpdateRange(models);
			ctx.SaveChanges();
		}

		// chats
		public static void AddChat(PhoneChatModel model)
		{
			using var ctx = new Context();
			ctx.PhoneChats.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveChat(PhoneChatModel model)
		{
			using var ctx = new Context();
			ctx.PhoneChats.Remove(model);
			ctx.SaveChanges();
		}

		public static PhoneChatModel? GetChat(int id)
		{
			using var ctx = new Context();
			return ctx.PhoneChats.FirstOrDefault(x => x.Id == id);
		}

		public static PhoneChatModel? GetChat(int account1, int account2)
		{
			using var ctx = new Context();
			return ctx.PhoneChats.FirstOrDefault(x => (x.Account1 == account1 && x.Account2 == account2) || (x.Account1 == account2 && x.Account2 == account1));
		}

		public static List<PhoneChatModel> GetChats(int account)
		{
			using var ctx = new Context();
			return ctx.PhoneChats.Where(x => x.Account1 == account || x.Account2 == account).ToList();
		}

		public static void UpdateChat(PhoneChatModel model)
		{
			using var ctx = new Context();
			ctx.PhoneChats.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateChats(IEnumerable<PhoneChatModel> models)
		{
			using var ctx = new Context();
			ctx.PhoneChats.UpdateRange(models);
			ctx.SaveChanges();
		}

		// chat messages
		public static void AddChatMessage(PhoneChatMessageModel model)
		{
			using var ctx = new Context();
			ctx.PhoneChatMessages.Add(model);
			ctx.SaveChanges();
		}

		public static void RemoveChatMessage(PhoneChatMessageModel model)
		{
			using var ctx = new Context();
			ctx.PhoneChatMessages.Remove(model);
			ctx.SaveChanges();
		}

		public static PhoneChatMessageModel? GetChatMessage(int id)
		{
			using var ctx = new Context();
			return ctx.PhoneChatMessages.FirstOrDefault(x => x.Id == id);
		}

		public static List<PhoneChatMessageModel> GetChatMessages(int chatId)
		{
			using var ctx = new Context();
			return ctx.PhoneChatMessages.Where(x => x.ChatId == chatId).ToList();
		}

		public static void UpdateChatMessage(PhoneChatMessageModel model)
		{
			using var ctx = new Context();
			ctx.PhoneChatMessages.Update(model);
			ctx.SaveChanges();
		}

		public static void UpdateChatMessages(IEnumerable<PhoneChatMessageModel> models)
		{
			using var ctx = new Context();
			ctx.PhoneChatMessages.UpdateRange(models);
			ctx.SaveChanges();
		}
	}
}