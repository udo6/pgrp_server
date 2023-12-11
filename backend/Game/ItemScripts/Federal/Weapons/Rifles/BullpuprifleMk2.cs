namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class BullpuprifleMk2 : WeaponItemScript
	{
		public BullpuprifleMk2() : base(178, 2228681469u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class BullpuprifleMk2Ammo : AmmoItemScript
	{
		public BullpuprifleMk2Ammo() : base(237, 2228681469u, 30, true)
		{
		}
	}
}