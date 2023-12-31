using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Crimes
{
    public class CrimeBaseModel
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public int GroupId { get; set; }
        public int JailTime { get; set; }
        public int Fine { get; set; }

        public CrimeBaseModel()
        {
            Label = string.Empty;
        }

        public CrimeBaseModel(string label, int group, int jailTime, int fine)
        {
            Label = label;
            GroupId = group;
            JailTime = jailTime;
            Fine = fine;
        }
    }

	public class CrimeBaseModelConfiguration : IEntityTypeConfiguration<CrimeBaseModel>
	{
		public void Configure(EntityTypeBuilder<CrimeBaseModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_crimes");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.Label).HasColumnName("label").HasColumnType("varchar(255)");
			builder.Property(x => x.GroupId).HasColumnName("group_id").HasColumnType("int(11)");
			builder.Property(x => x.JailTime).HasColumnName("jailtime").HasColumnType("int(11)");
			builder.Property(x => x.Fine).HasColumnName("fine").HasColumnType("int(11)");
		}
	}
}