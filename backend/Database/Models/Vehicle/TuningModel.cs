using AltV.Net.Data;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Vehicle
{
    public class TuningModel
    {
        public int Id { get; set; }

        public byte PrimaryColor { get; set; }
        public byte SecondaryColor { get; set; }
        public byte PearlColor { get; set; }
        public byte Spoiler { get; set; }
        public byte FrontBumper { get; set; }
        public byte RearBumper { get; set; }
        public byte SideSkirt { get; set; }
        public byte Exhaust { get; set; }
        public byte Frame { get; set; }
        public byte Grille { get; set; }
        public byte Hood { get; set; }
        public byte Fender { get; set; }
        public byte RightFender { get; set; }
        public byte Roof { get; set; }
        public byte Engine { get; set; }
        public byte Brakes { get; set; }
        public byte Transmission { get; set; }
        public byte Horns { get; set; }
        public byte Suspension { get; set; }
        public byte Armor { get; set; }
        public byte Turbo { get; set; }
        public byte Xenon { get; set; }
        public byte Wheels { get; set; }
		public byte WheelType { get; set; }
		public byte WheelColor { get; set; }
		public byte PlateHolders { get; set; }
        public byte TrimDesign { get; set; }
        public byte WindowTint { get; set; }
        public byte HeadlightColor { get; set; }
        public byte Livery { get; set; }
        public bool Neons { get; set; }
        public byte NeonR { get; set; }
        public byte NeonG { get; set; }
        public byte NeonB { get; set; }

        public TuningModel()
        {
        }

        public TuningModel(byte primaryColor, byte secondaryColor, byte pearlColor, byte spoiler, byte frontBumper, byte rearBumper, byte sideSkirt, byte exhaust, byte frame, byte grille, byte hood, byte fender, byte rightFender, byte roof, byte engine, byte brakes, byte transmission, byte horn, byte suspension, byte armor, byte turbo, byte wheels, byte wheelType, byte wheelColor, byte plateHolders, byte trimDesign, byte windowTint, byte headlightColor, byte livery, bool neons, byte neonR, byte neonG, byte neonB)
        {
            PrimaryColor = primaryColor;
            SecondaryColor = secondaryColor;
            PearlColor = pearlColor;
            Spoiler = spoiler;
            FrontBumper = frontBumper;
            RearBumper = rearBumper;
            SideSkirt = sideSkirt;
            Exhaust = exhaust;
            Frame = frame;
            Grille = grille;
            Hood = hood;
            Fender = fender;
            RightFender = rightFender;
            Roof = roof;
            Engine = engine;
            Brakes = brakes;
            Transmission = transmission;
            Horns = horn;
            Suspension = suspension;
            Armor = armor;
            Turbo = turbo;
            Wheels = wheels;
            WheelType = wheelType;
            WheelColor = wheelColor;
            PlateHolders = plateHolders;
            TrimDesign = trimDesign;
            WindowTint = windowTint;
            HeadlightColor = headlightColor;
            Livery = livery;
            Neons = neons;
            NeonR = neonR;
            NeonG = neonG;
            NeonB = neonB;
        }
	}

	public class TuningModelConfiguration : IEntityTypeConfiguration<TuningModel>
	{
		public void Configure(EntityTypeBuilder<TuningModel> builder)
		{
			builder.HasKey(x => x.Id);
			builder.ToTable("server_vehicle_tuning");
			builder.HasIndex(x => x.Id).HasDatabaseName("id");
			builder.Property(x => x.Id).HasColumnName("id").HasColumnType("int(11)");
			builder.Property(x => x.PrimaryColor).HasColumnName("primary_color").HasColumnType("byte");
			builder.Property(x => x.SecondaryColor).HasColumnName("secondary_color").HasColumnType("byte");
			builder.Property(x => x.PearlColor).HasColumnName("pearl_color").HasColumnType("byte");
			builder.Property(x => x.Spoiler).HasColumnName("spoiler").HasColumnType("byte");
			builder.Property(x => x.FrontBumper).HasColumnName("front_bumper").HasColumnType("byte");
			builder.Property(x => x.RearBumper).HasColumnName("rear_bumper").HasColumnType("byte");
			builder.Property(x => x.SideSkirt).HasColumnName("side_skirt").HasColumnType("byte");
			builder.Property(x => x.Exhaust).HasColumnName("exhaust").HasColumnType("byte");
			builder.Property(x => x.Frame).HasColumnName("frame").HasColumnType("byte");
			builder.Property(x => x.Grille).HasColumnName("grille").HasColumnType("byte");
			builder.Property(x => x.Hood).HasColumnName("hood").HasColumnType("byte");
			builder.Property(x => x.Fender).HasColumnName("fender").HasColumnType("byte");
			builder.Property(x => x.RightFender).HasColumnName("right_fender").HasColumnType("byte");
			builder.Property(x => x.Roof).HasColumnName("roof").HasColumnType("byte");
			builder.Property(x => x.Engine).HasColumnName("engine").HasColumnType("byte");
			builder.Property(x => x.Brakes).HasColumnName("brakes").HasColumnType("byte");
			builder.Property(x => x.Transmission).HasColumnName("transmission").HasColumnType("byte");
			builder.Property(x => x.Horns).HasColumnName("horns").HasColumnType("byte");
			builder.Property(x => x.Suspension).HasColumnName("suspension").HasColumnType("byte");
			builder.Property(x => x.Armor).HasColumnName("armor").HasColumnType("byte");
			builder.Property(x => x.Turbo).HasColumnName("turbo").HasColumnType("byte");
			builder.Property(x => x.Xenon).HasColumnName("xenon").HasColumnType("byte");
			builder.Property(x => x.Wheels).HasColumnName("wheels").HasColumnType("byte");
			builder.Property(x => x.WheelType).HasColumnName("wheel_type").HasColumnType("byte");
			builder.Property(x => x.WheelColor).HasColumnName("wheel_color").HasColumnType("byte");
			builder.Property(x => x.PlateHolders).HasColumnName("plate_holders").HasColumnType("byte");
			builder.Property(x => x.TrimDesign).HasColumnName("trim_design").HasColumnType("byte");
			builder.Property(x => x.WindowTint).HasColumnName("window_tint").HasColumnType("byte");
			builder.Property(x => x.HeadlightColor).HasColumnName("headlight_color").HasColumnType("byte");
			builder.Property(x => x.Livery).HasColumnName("livery").HasColumnType("byte");
			builder.Property(x => x.Neons).HasColumnName("neons").HasColumnType("tinyint(1)");
			builder.Property(x => x.NeonR).HasColumnName("neon_r").HasColumnType("byte");
			builder.Property(x => x.NeonG).HasColumnName("neon_g").HasColumnType("byte");
			builder.Property(x => x.NeonB).HasColumnName("neon_b").HasColumnType("byte");
		}
	}
}