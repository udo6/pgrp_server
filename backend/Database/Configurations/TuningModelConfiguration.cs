using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Database.Models.Vehicle;

namespace Database.Configurations
{
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