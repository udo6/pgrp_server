using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database.Models.Tuner
{
	public class TunerCategoryModel
	{
		public int Id { get; set; }
		public string Name { get; set; }

		public TunerCategoryModel()
		{
			Name = string.Empty;
		}

		public TunerCategoryModel(string name)
		{
			Name = name;
		}
	}

	public class TunerCategoryModelConfiguration : IEntityTypeConfiguration<TunerCategoryModel>
	{
		public void Configure(EntityTypeBuilder<TunerCategoryModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_tuner_categories");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Name).HasColumnName("name").HasColumnType("varchar(255)");
		}
	}
}
