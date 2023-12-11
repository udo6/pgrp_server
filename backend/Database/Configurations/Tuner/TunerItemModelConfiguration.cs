using Database.Models.Tuner;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Configurations.Tuner
{
	public class TunerItemModelConfiguration : IEntityTypeConfiguration<TunerItemModel>
	{
		public void Configure(EntityTypeBuilder<TunerItemModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_tuner_items");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.CategoryId).HasColumnName("category_id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
			builder.Property(x => x.ModCategory).HasColumnName("mod_category").HasColumnType("int(11)");
			builder.Property(x => x.ModValue).HasColumnName("mod_value").HasColumnType("int(11)");
		}
	}
}
