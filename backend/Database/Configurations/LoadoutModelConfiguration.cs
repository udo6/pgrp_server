using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Account;

namespace Database.Configurations
{
    internal class LoadoutModelConfiguration : IEntityTypeConfiguration<LoadoutModel>
	{
		public void Configure(EntityTypeBuilder<LoadoutModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_loadouts");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.Hash).HasColumnName("hash").HasColumnType("uint(11)");
			builder.Property(x => x.Ammo).HasColumnName("ammo").HasColumnType("int(11)");
			builder.Property(x => x.TintIndex).HasColumnName("tint_index").HasColumnType("int(11)");
			builder.Property(x => x.Type).HasColumnName("type").HasColumnType("int(11)");
		}
	}
}