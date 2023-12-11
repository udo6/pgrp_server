namespace Game.ItemScripts.Federal.Weapons.Shotguns
{
	public class SweeperShotgun : WeaponItemScript
	{
		public SweeperShotgun() : base(271, 317205821, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class SweeperShotgunAmmo : AmmoItemScript
	{
		public SweeperShotgunAmmo() : base(272, 317205821, 10, true)
		{
		}
	}
}