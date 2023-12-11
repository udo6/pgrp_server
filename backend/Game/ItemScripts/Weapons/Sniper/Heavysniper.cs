namespace Game.ItemScripts.Weapons.Sniper
{
	public class Heavysniper : WeaponItemScript
	{
		public Heavysniper() : base(87, 205991906, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class HeavysniperAmmo : AmmoItemScript
	{
		public HeavysniperAmmo() : base(128, 205991906, 6)
		{
		}
	}
}