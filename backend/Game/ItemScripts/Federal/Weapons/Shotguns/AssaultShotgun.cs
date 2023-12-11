namespace Game.ItemScripts.Federal.Weapons.Shotguns
{
	public class AssaultShotgun : WeaponItemScript
	{
		public AssaultShotgun() : base(265, 3800352039, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class AssaultShotgunAmmo : AmmoItemScript
	{
		public AssaultShotgunAmmo() : base(266, 3800352039, 8, true)
		{
		}
	}
}