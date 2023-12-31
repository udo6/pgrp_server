using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Account
{
	public class TattooModel
	{
		public int Id { get; set; }
		public int AccountId { get; set; }
		public uint Collection { get; set; }
		public uint Overlay { get; set; }

		public TattooModel()
		{
		}

		public TattooModel(int accountId, uint collection, uint overlay)
		{
			AccountId = accountId;
			Collection = collection;
			Overlay = overlay;
		}
	}

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