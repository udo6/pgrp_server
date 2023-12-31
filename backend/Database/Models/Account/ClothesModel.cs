using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Database.Models.Account
{
    public class ClothesModel
    {
        public int Id { get; set; }

        public int Mask { get; set; }
        public int MaskColor { get; set; }
        public uint MaskDlc { get; set; }
        public int Top { get; set; }
        public int TopColor { get; set; }
        public uint TopDlc { get; set; }
        public int Body { get; set; }
        public int BodyColor { get; set; }
        public uint BodyDlc { get; set; }
        public int Undershirt { get; set; }
        public int UndershirtColor { get; set; }
        public uint UndershirtDlc { get; set; }
        public int Pants { get; set; }
        public int PantsColor { get; set; }
        public uint PantsDlc { get; set; }
        public int Shoes { get; set; }
        public int ShoesColor { get; set; }
        public uint ShoesDlc { get; set; }
        public int Accessories { get; set; }
        public int AccessoriesColor { get; set; }
        public uint AccessoriesDlc { get; set; }
        public int Armor { get; set; }
        public int ArmorColor { get; set; }
        public uint ArmorDlc { get; set; }
        public int Decals { get; set; }
        public int DecalsColor { get; set; }
        public uint DecalsDlc { get; set; }

        public int Hat { get; set; }
        public int HatColor { get; set; }
        public uint HatDlc { get; set; }
        public int Glasses { get; set; }
        public int GlassesColor { get; set; }
        public uint GlassesDlc { get; set; }
        public int Ears { get; set; }
        public int EarsColor { get; set; }
        public uint EarsDlc { get; set; }
        public int Watch { get; set; }
        public int WatchColor { get; set; }
        public uint WatchDlc { get; set; }
        public int Bracelet { get; set; }
        public int BraceletColor { get; set; }
        public uint BraceletDlc { get; set; }

        public ClothesModel()
        {
            Hat = -1;
            Glasses = -1;
            Ears = -1;
            Watch = -1;
            Bracelet = -1;
        }

        public ClothesModel(int mask, int maskColor, uint maskDlc, int top, int topColor, uint topDlc, int body, int bodyColor, uint bodyDlc, int undershirt, int undershirtColor, uint undershirtDlc, int pants, int pantsColor, uint pantsDlc, int shoes, int shoesColor, uint shoesDlc, int accessories, int accessoriesColor, uint accessoriesDlc, int armor, int armorColor, uint armorDlc, int decals, int decalsColor, uint decalsDlc, int hat, int hatColor, uint hatDlc, int glasses, int glassesColor, uint glassesDlc, int ears, int earsColor, uint earsDlc, int watch, int watchColor, uint watchDlc, int bracelet, int braceletColor, uint braceletDlc)
        {
            Mask = mask;
            MaskColor = maskColor;
            MaskDlc = maskDlc;
            Top = top;
            TopColor = topColor;
            TopDlc = topDlc;
            Body = body;
            BodyColor = bodyColor;
            BodyDlc = bodyDlc;
            Undershirt = undershirt;
            UndershirtColor = undershirtColor;
            UndershirtDlc = undershirtDlc;
            Pants = pants;
            PantsColor = pantsColor;
            PantsDlc = pantsDlc;
            Shoes = shoes;
            ShoesColor = shoesColor;
            ShoesDlc = shoesDlc;
            Accessories = accessories;
            AccessoriesColor = accessoriesColor;
            AccessoriesDlc = accessoriesDlc;
            Armor = armor;
            ArmorColor = armorColor;
            ArmorDlc = armorDlc;
            Decals = decals;
            DecalsColor = decalsColor;
            DecalsDlc = decalsDlc;
            Hat = hat;
            HatColor = hatColor;
            HatDlc = hatDlc;
            Glasses = glasses;
            GlassesColor = glassesColor;
            GlassesDlc = glassesDlc;
            Ears = ears;
            EarsColor = earsColor;
            EarsDlc = earsDlc;
            Watch = watch;
            WatchColor = watchColor;
            WatchDlc = watchDlc;
            Bracelet = bracelet;
            BraceletColor = braceletColor;
            BraceletDlc = braceletDlc;
        }

        public void CopyTo(ClothesModel model)
        {
            model.Mask = Mask;
            model.MaskColor = MaskColor;
            model.MaskDlc = MaskDlc;
            model.Top = Top;
            model.TopColor = TopColor;
            model.TopDlc = TopDlc;
            model.Body = Body;
            model.BodyColor = BodyColor;
            model.BodyDlc = BodyDlc;
            model.Undershirt = Undershirt;
            model.UndershirtColor = UndershirtColor;
            model.UndershirtDlc = UndershirtDlc;
            model.Pants = Pants;
            model.PantsColor = PantsColor;
            model.PantsDlc = PantsDlc;
            model.Shoes = Shoes;
            model.ShoesColor = ShoesColor;
            model.ShoesDlc = ShoesDlc;
            model.Accessories = Accessories;
            model.AccessoriesColor = AccessoriesColor;
            model.AccessoriesDlc = AccessoriesDlc;
            model.Armor = Armor;
            model.ArmorColor = ArmorColor;
            model.ArmorDlc = ArmorDlc;
            model.Decals = Decals;
            model.DecalsColor = DecalsColor;
            model.DecalsDlc = DecalsDlc;
            model.Hat = Hat;
            model.HatColor = HatColor;
            model.HatDlc = HatDlc;
            model.Glasses = Glasses;
            model.GlassesColor = GlassesColor;
            model.GlassesDlc = GlassesDlc;
            model.Ears = Ears;
            model.EarsColor = EarsColor;
            model.EarsDlc = EarsDlc;
            model.Watch = Watch;
            model.WatchColor = WatchColor;
            model.WatchDlc = WatchDlc;
            model.Bracelet = Bracelet;
            model.BraceletColor = BraceletColor;
            model.BraceletDlc = BraceletDlc;
        }
    }

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