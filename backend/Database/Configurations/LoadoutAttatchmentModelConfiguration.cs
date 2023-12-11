using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Account;

namespace Database.Configurations
{
    internal class LoadoutAttatchmentModelConfiguration : IEntityTypeConfiguration<LoadoutAttatchmentModel>
	{
		public void Configure(EntityTypeBuilder<LoadoutAttatchmentModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_loadout_attatchments");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.LoadoutId).HasColumnName("loadout_id").HasColumnType("int(11)");
			builder.Property(x => x.Hash).HasColumnName("hash").HasColumnType("uint(11)");
		}
	}
}