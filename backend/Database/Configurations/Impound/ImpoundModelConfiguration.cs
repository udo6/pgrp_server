using Database.Models.Account;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.DPOS;

namespace Database.Configurations.DPOS
{
	public class ImpoundModelConfiguration : IEntityTypeConfiguration<ImpoundModel>
	{
		public void Configure(EntityTypeBuilder<ImpoundModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_impounds");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PositionId).HasColumnName("position_id").HasColumnType("int(11)");
		}
	}
}