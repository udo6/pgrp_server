namespace Database.Models.Phone
{
    public class PhoneChatMessageModel
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public int Sender { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }

        public PhoneChatMessageModel()
        {
            Message = string.Empty;
        }

        public PhoneChatMessageModel(int chatId, int sender, string message, DateTime date)
        {
            ChatId = chatId;
            Sender = sender;
            Message = message;
            Date = date;
        }
    }
}