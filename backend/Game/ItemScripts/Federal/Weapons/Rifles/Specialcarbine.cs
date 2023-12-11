namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Specialcarbine : WeaponItemScript
	{
		public Specialcarbine() : base(184, 3231910285u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class SpecialcarbineAmmo : AmmoItemScript
	{
		public SpecialcarbineAmmo() : base(243, 3231910285u, 30, true)
		{
		}
	}
}