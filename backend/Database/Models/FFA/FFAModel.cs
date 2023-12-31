using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.FFA
{
	public class FFAModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public int MaxPlayers { get; set; }

		public FFAModel()
		{
			Name = string.Empty;
		}

		public FFAModel(string name, int maxPlayers)
		{
			Name = name;
			MaxPlayers = maxPlayers;
		}
	}

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