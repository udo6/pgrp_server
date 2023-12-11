namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class CarbinerifleMk2 : WeaponItemScript
	{
		public CarbinerifleMk2() : base(180, 4208062921u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class CarbinerifleMk2Ammo : AmmoItemScript
	{
		public CarbinerifleMk2Ammo() : base(239, 4208062921u, 30, true)
		{
		}
	}
}