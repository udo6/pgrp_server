using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Account;

namespace Database.Configurations
{
    public class ClothesModelConfiguration : IEntityTypeConfiguration<ClothesModel>
	{
		public void Configure(EntityTypeBuilder<ClothesModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_account_clothes");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");

			builder.Property(x => x.Mask).HasColumnName("mask").HasColumnType("int(11)");
			builder.Property(x => x.MaskColor).HasColumnName("mask_color").HasColumnType("int(11)");
			builder.Property(x => x.MaskDlc).HasColumnName("mask_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Top).HasColumnName("top").HasColumnType("int(11)");
			builder.Property(x => x.TopColor).HasColumnName("top_color").HasColumnType("int(11)");
			builder.Property(x => x.TopDlc).HasColumnName("top_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Body).HasColumnName("body").HasColumnType("int(11)");
			builder.Property(x => x.BodyColor).HasColumnName("body_color").HasColumnType("int(11)");
			builder.Property(x => x.BodyDlc).HasColumnName("body_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Undershirt).HasColumnName("undershirt").HasColumnType("int(11)");
			builder.Property(x => x.UndershirtColor).HasColumnName("undershirt_color").HasColumnType("int(11)");
			builder.Property(x => x.UndershirtDlc).HasColumnName("undershirt_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Pants).HasColumnName("pants").HasColumnType("int(11)");
			builder.Property(x => x.PantsColor).HasColumnName("pants_color").HasColumnType("int(11)");
			builder.Property(x => x.PantsDlc).HasColumnName("pants_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Shoes).HasColumnName("shoes").HasColumnType("int(11)");
			builder.Property(x => x.ShoesColor).HasColumnName("shoes_color").HasColumnType("int(11)");
			builder.Property(x => x.ShoesDlc).HasColumnName("shoes_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Accessories).HasColumnName("accessories").HasColumnType("int(11)");
			builder.Property(x => x.AccessoriesColor).HasColumnName("accessories_color").HasColumnType("int(11)");
			builder.Property(x => x.AccessoriesDlc).HasColumnName("accessories_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Armor).HasColumnName("armor").HasColumnType("int(11)");
			builder.Property(x => x.ArmorColor).HasColumnName("armor_color").HasColumnType("int(11)");
			builder.Property(x => x.ArmorDlc).HasColumnName("armor_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Decals).HasColumnName("decals").HasColumnType("int(11)");
			builder.Property(x => x.DecalsColor).HasColumnName("decals_color").HasColumnType("int(11)");
			builder.Property(x => x.DecalsDlc).HasColumnName("decals_dlc").HasColumnType("int(11)");

			builder.Property(x => x.Hat).HasColumnName("hat").HasColumnType("int(11)");
			builder.Property(x => x.HatColor).HasColumnName("hat_color").HasColumnType("int(11)");
			builder.Property(x => x.HatDlc).HasColumnName("hat_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Glasses).HasColumnName("glasses").HasColumnType("int(11)");
			builder.Property(x => x.GlassesColor).HasColumnName("glasses_color").HasColumnType("int(11)");
			builder.Property(x => x.GlassesDlc).HasColumnName("glasses_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Ears).HasColumnName("ears").HasColumnType("int(11)");
			builder.Property(x => x.EarsColor).HasColumnName("ears_color").HasColumnType("int(11)");
			builder.Property(x => x.EarsDlc).HasColumnName("ears_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Watch).HasColumnName("watch").HasColumnType("int(11)");
			builder.Property(x => x.WatchColor).HasColumnName("watch_color").HasColumnType("int(11)");
			builder.Property(x => x.WatchDlc).HasColumnName("watch_dlc").HasColumnType("int(11)");
			builder.Property(x => x.Bracelet).HasColumnName("bracelet").HasColumnType("int(11)");
			builder.Property(x => x.BraceletColor).HasColumnName("bracelet_color").HasColumnType("int(11)");
			builder.Property(x => x.BraceletDlc).HasColumnName("bracelet_dlc").HasColumnType("int(11)");
		}
	}
}