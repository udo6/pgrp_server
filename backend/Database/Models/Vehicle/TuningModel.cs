using AltV.Net.Data;
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
}