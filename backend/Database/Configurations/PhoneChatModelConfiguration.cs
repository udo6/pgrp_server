using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Phone;

namespace Database.Configurations
{
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