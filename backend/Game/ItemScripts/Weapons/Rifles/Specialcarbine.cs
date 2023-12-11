namespace Game.ItemScripts.Weapons.Rifles
{
	public class Specialcarbine : WeaponItemScript
	{
		public Specialcarbine() : base(58, 3231910285u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class SpecialcarbineAmmo : AmmoItemScript
	{
		public SpecialcarbineAmmo() : base(117, 3231910285u, 30)
		{
		}
	}
}