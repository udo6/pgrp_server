namespace Game.ItemScripts.Federal.Weapons.Pistol
{
	public class Flaregun : WeaponItemScript
	{
		public Flaregun() : base(189, 1198879012, true, Core.Enums.InjuryType.SHOT_LOW, Core.Enums.WeaponType.PISTOL)
		{
		}
	}

	public class FlaregunAmmo : AmmoItemScript
	{
		public FlaregunAmmo() : base(222, 1198879012, 20, true)
		{
		}
	}
}