namespace Game.ItemScripts.Weapons.Shotguns
{
	public class SweeperShotgun : WeaponItemScript
	{
		public SweeperShotgun() : base(145, 317205821, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class SweeperShotgunAmmo : AmmoItemScript
	{
		public SweeperShotgunAmmo() : base(146, 317205821, 10)
		{
		}
	}
}