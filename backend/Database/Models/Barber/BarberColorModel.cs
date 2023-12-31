using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Barber
{
	public class BarberColorModel
	{
		public int Id { get; set; }
		public string HexColor { get; set; }
		public int Value { get; set; }
		public int Price { get; set; }

		public BarberColorModel()
		{
			HexColor = string.Empty;
		}

		public BarberColorModel(string hexColor, int value, int price)
		{
			HexColor = hexColor;
			Value = value;
			Price = price;
		}
	}

	public class BarberColorModelConfiguration : IEntityTypeConfiguration<BarberColorModel>
	{
		public void Configure(EntityTypeBuilder<BarberColorModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_barber_colors");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.HexColor).HasColumnName("hex_color").HasColumnType("varchar(255)");
			builder.Property(x => x.Value).HasColumnName("value").HasColumnType("int(11)");
			builder.Property(x => x.Price).HasColumnName("price").HasColumnType("int(11)");
		}
	}
}