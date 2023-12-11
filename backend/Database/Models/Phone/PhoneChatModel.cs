namespace Database.Models.Phone
{
    public class PhoneChatModel
    {
        public int Id { get; set; }
        public int Account1 { get; set; }
        public int Account2 { get; set; }
        public int LastChatMessageId { get; set; }

        public PhoneChatModel()
        {

        }

        public PhoneChatModel(int account1, int account2, int lastChatMessageId)
        {
            Account1 = account1;
            Account2 = account2;
            LastChatMessageId = lastChatMessageId;
        }
    }
}