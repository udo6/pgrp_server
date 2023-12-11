using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.FFA;

namespace Database.Configurations.FFA
{
	public class FFAModelConfiguration : IEntityTypeConfiguration<FFAModel>
	{
		public void Configure(EntityTypeBuilder<FFAModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_ffas");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.MaxPlayers).HasColumnName("max_players").HasColumnType("int(11)");
		}
	}
}