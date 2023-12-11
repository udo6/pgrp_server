namespace Game.ItemScripts.Weapons.Shotguns
{
	public class BullpupShotgun : WeaponItemScript
	{
		public BullpupShotgun() : base(141, 2640438543, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class BullpupShotgunAmmo : AmmoItemScript
	{
		public BullpupShotgunAmmo() : base(142, 2640438543, 14)
		{
		}
	}
}