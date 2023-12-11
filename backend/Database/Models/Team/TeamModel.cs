using AltV.Net.Data;
using Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Models.Team
{
    public class TeamModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortName { get; set; }
        public int PositionId { get; set; }

        public byte ColorR { get; set; }
        public byte ColorG { get; set; }
        public byte ColorB { get; set; }
        [NotMapped]
        public Rgba Color => new(ColorR, ColorG, ColorB, 255);

        public byte BlipColor { get; set; }
        public TeamType Type { get; set; }
        public int Warns { get; set; }
        public string MeeleWeapon { get; set; }
        public uint MeeleWeaponHash { get; set; }
        public int Money { get; set; }

        public TeamModel()
        {
            Name = string.Empty;
            ShortName = string.Empty;
            MeeleWeapon = string.Empty;
        }

        public TeamModel(string name, string shortName, int positionId, byte colorR, byte colorG, byte colorB, byte blipColor, TeamType type, int warns, string meeleWeapon, uint meeleWeaponHash, int money)
        {
            Name = name;
            ShortName = shortName;
            PositionId = positionId;
            ColorR = colorR;
            ColorG = colorG;
            ColorB = colorB;
            BlipColor = blipColor;
            Type = type;
            Warns = warns;
            MeeleWeapon = meeleWeapon;
            MeeleWeaponHash = meeleWeaponHash;
            Money = money;
        }
    }
}