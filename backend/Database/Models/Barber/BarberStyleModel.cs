using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Barber
{
	public class BarberStyleModel
	{
		public int Id { get; set; }
		public int BarberId { get; set; }
		public string Label { get; set; }
		public int Value { get; set; }
		public int Price { get; set; }
		public int Gender { get; set; }

		public BarberStyleModel()
		{
			Label = string.Empty;
		}

		public BarberStyleModel(int barberId, string label, int value, int price, int gender)
		{
			BarberId = barberId;
			Label = label;
			Value = value;
			Price = price;
			Gender = gender;
		}
	}

	public class BarberStyleModelConfiguration : IEntityTypeConfiguration<BarberStyleModel>
	{
		public void Configure(EntityTypeBuilder<BarberStyleModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_barber_styles");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.BarberId).HasColumnName("barber_id").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.Value).HasColumnName("value").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
		}
	}
}