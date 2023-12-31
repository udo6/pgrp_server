using Core.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Account
{
    public class LoadoutModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public uint Hash { get; set; }
        public int Ammo { get; set; }
        public int TintIndex { get; set; }
        public LoadoutType Type { get; set; }

        public LoadoutModel() { }

        public LoadoutModel(int accountId, uint hash, int ammo, int tintIndex, LoadoutType type)
        {
            AccountId = accountId;
            Hash = hash;
            Ammo = ammo;
            TintIndex = tintIndex;
            Type = type;
        }
    }

	public class LoadoutModelConfiguration : IEntityTypeConfiguration<LoadoutModel>
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