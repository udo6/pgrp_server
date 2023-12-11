using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Animation;

namespace Database.Configurations
{
    public class AnimationFavoriteConfiguration : IEntityTypeConfiguration<AnimationFavoriteModel>
	{
		public void Configure(EntityTypeBuilder<AnimationFavoriteModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_animation_favorites");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.AccountId).HasColumnName("account_id").HasColumnType("int(11)");
			builder.Property(x => x.AnimationId).HasColumnName("animation_id").HasColumnType("int(11)");
			builder.Property(x => x.Slot).HasColumnName("slot").HasColumnType("int(11)");
		}
	}
}