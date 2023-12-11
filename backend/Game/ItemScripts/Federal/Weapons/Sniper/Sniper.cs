namespace Game.ItemScripts.Federal.Weapons.Sniper
{
	public class Sniper : WeaponItemScript
	{
		public Sniper() : base(212, 100416529, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class SniperAmmo : AmmoItemScript
	{
		public SniperAmmo() : base(258, 100416529, 10, true)
		{
		}
	}
}