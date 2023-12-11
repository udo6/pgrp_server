namespace Game.ItemScripts.Weapons.Sniper
{
	public class HeavysniperMk2 : WeaponItemScript
	{
		public HeavysniperMk2() : base(88, 177293209, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class HeavysniperMk2Ammo : AmmoItemScript
	{
		public HeavysniperMk2Ammo() : base(129, 177293209, 6)
		{
		}
	}
}