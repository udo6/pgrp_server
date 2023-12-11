namespace Game.ItemScripts.Weapons.Pistol
{
	public class Flaregun : WeaponItemScript
	{
		public Flaregun() : base(63, 1198879012, false, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class FlaregunAmmo : AmmoItemScript
	{
		public FlaregunAmmo() : base(96, 1198879012, 20)
		{
		}
	}
}