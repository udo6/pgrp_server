namespace Game.ItemScripts.Weapons.Sniper
{
	public class Sniper : WeaponItemScript
	{
		public Sniper() : base(86, 100416529, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class SniperAmmo : AmmoItemScript
	{
		public SniperAmmo() : base(132, 100416529, 10)
		{
		}
	}
}