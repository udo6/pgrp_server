namespace Game.ItemScripts.Weapons.Rifles
{
	public class BullpuprifleMk2 : WeaponItemScript
	{
		public BullpuprifleMk2() : base(52, 2228681469u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class BullpuprifleMk2Ammo : AmmoItemScript
	{
		public BullpuprifleMk2Ammo() : base(111, 2228681469u, 30)
		{
		}
	}
}