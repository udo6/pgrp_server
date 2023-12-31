using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

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

	public class PhoneChatModelConfiguration : IEntityTypeConfiguration<PhoneChatModel>
	{
		public void Configure(EntityTypeBuilder<PhoneChatModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_phone_chats");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Account1).HasColumnName("account_1").HasColumnType("int(11)");
			builder.Property(x => x.Account2).HasColumnName("account_2").HasColumnType("int(11)");
			builder.Property(x => x.LastChatMessageId).HasColumnName("last_chat_id").HasColumnType("int(11)");
		}
	}
}