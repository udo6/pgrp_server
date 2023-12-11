namespace Game.ItemScripts.Weapons.Shotguns
{
	public class AssaultShotgun : WeaponItemScript
	{
		public AssaultShotgun() : base(139, 3800352039, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class AssaultShotgunAmmo : AmmoItemScript
	{
		public AssaultShotgunAmmo() : base(140, 3800352039, 8)
		{
		}
	}
}