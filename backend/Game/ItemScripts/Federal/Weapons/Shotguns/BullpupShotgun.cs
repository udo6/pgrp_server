namespace Game.ItemScripts.Federal.Weapons.Shotguns
{
	public class BullpupShotgun : WeaponItemScript
	{
		public BullpupShotgun() : base(267, 2640438543, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SHOTGUN)
		{
		}
	}

	public class BullpupShotgunAmmo : AmmoItemScript
	{
		public BullpupShotgunAmmo() : base(268, 2640438543, 14, true)
		{
		}
	}
}