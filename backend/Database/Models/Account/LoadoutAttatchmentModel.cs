using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Account
{
    public class LoadoutAttatchmentModel
    {
        public int Id { get; set; }
        public int LoadoutId { get; set; }
        public uint Hash { get; set; }

        public LoadoutAttatchmentModel()
        {

        }

        public LoadoutAttatchmentModel(int loadoutId, uint hash)
        {
            LoadoutId = loadoutId;
            Hash = hash;
        }
    }

	public class LoadoutAttatchmentModelConfiguration : IEntityTypeConfiguration<LoadoutAttatchmentModel>
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