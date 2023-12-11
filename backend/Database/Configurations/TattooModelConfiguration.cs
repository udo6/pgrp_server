using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Configurations
{
	public class TattooModelConfiguration : IEntityTypeConfiguration<TattooModel>
	{
		public void Configure(EntityTypeBuilder<TattooModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_tattoos");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.Collection).HasColumnName("collection").HasColumnType("int(11)");
			builder.Property(x => x.Overlay).HasColumnName("overlay").HasColumnType("int(11)");
		}
	}
}