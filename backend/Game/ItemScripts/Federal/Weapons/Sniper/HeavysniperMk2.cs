namespace Game.ItemScripts.Federal.Weapons.Sniper
{
	public class HeavysniperMk2 : WeaponItemScript
	{
		public HeavysniperMk2() : base(214, 177293209, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class HeavysniperMk2Ammo : AmmoItemScript
	{
		public HeavysniperMk2Ammo() : base(255, 177293209, 6, true)
		{
		}
	}
}