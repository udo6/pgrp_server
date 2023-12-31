using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Crimes
{
    public class CrimeModel
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int CrimeId { get; set; }
        public string Created { get; set; }
        public string OfficerName { get; set; }

        public CrimeModel()
        {
            Created = string.Empty;
            OfficerName = string.Empty;
        }

        public CrimeModel(int accountId, int crimeId, string created, string officerName)
        {
            AccountId = accountId;
            CrimeId = crimeId;
            Created = created;
            OfficerName = officerName;
        }
    }

	public class CrimeModelConfiguration : IEntityTypeConfiguration<CrimeModel>
	{
		public void Configure(EntityTypeBuilder<CrimeModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_crimes");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.CrimeId).HasColumnName("crime_id").HasColumnType("int(11)");
			builder.Property(x => x.Created).HasColumnName("created").HasColumnType("varchar(255)");
			builder.Property(x => x.OfficerName).HasColumnName("officer").HasColumnType("varchar(255)");
		}
	}
}