using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

	public class PhoneChatMessageModelConfiguration : IEntityTypeConfiguration<PhoneChatMessageModel>
	{
		public void Configure(EntityTypeBuilder<PhoneChatMessageModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_phone_chat_messages");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.ChatId).HasColumnName("chat_id").HasColumnType("int(11)");
			builder.Property(x => x.Sender).HasColumnName("sender").HasColumnType("int(11)");
			builder.Property(x => x.Message).HasColumnName("message").HasColumnType("longtext");
			builder.Property(x => x.Date).HasColumnName("date").HasColumnType("datetime");
		}
	}
}