using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Phone;

namespace Database.Configurations
{
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