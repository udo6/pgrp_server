using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Phone;

namespace Database.Configurations
{
    public class PhoneContactModelConfiguration : IEntityTypeConfiguration<PhoneContactModel>
	{
		public void Configure(EntityTypeBuilder<PhoneContactModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_phone_contacts");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.Number).HasColumnName("number").HasColumnType("int(11)");
			builder.Property(x => x.Favorite).HasColumnName("favorite").HasColumnType("tinyint(1)");
		}
	}
}