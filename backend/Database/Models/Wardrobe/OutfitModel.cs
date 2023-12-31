using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Wardrobe
{
    public class OutfitModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AccountId { get; set; }
        public int ClothesId { get; set; }

        public OutfitModel()
        {
            Name = string.Empty;
        }

        public OutfitModel(string name, int accountId, int clothesId)
        {
            Name = name;
            AccountId = accountId;
            ClothesId = clothesId;
        }
    }

	public class OutfitModelConfiguration : IEntityTypeConfiguration<OutfitModel>
	{
		public void Configure(EntityTypeBuilder<OutfitModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_wardrobe_outfits");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.ClothesId).HasColumnName("clothes_id").HasColumnType("int(11)");
		}
	}
}