namespace Game.ItemScripts.Federal.Weapons.Sniper
{
	public class Heavysniper : WeaponItemScript
	{
		public Heavysniper() : base(213, 205991906, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class HeavysniperAmmo : AmmoItemScript
	{
		public HeavysniperAmmo() : base(254, 205991906, 6, true)
		{
		}
	}
}