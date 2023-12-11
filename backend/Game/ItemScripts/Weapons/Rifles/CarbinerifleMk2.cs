namespace Game.ItemScripts.Weapons.Rifles
{
	public class CarbinerifleMk2 : WeaponItemScript
	{
		public CarbinerifleMk2() : base(54, 4208062921u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class CarbinerifleMk2Ammo : AmmoItemScript
	{
		public CarbinerifleMk2Ammo() : base(113, 4208062921u, 30)
		{
		}
	}
}