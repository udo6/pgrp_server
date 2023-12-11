namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Carbinerifle : WeaponItemScript
	{
		public Carbinerifle() : base(179, 2210333304u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class CarbinerifleAmmo : AmmoItemScript
	{
		public CarbinerifleAmmo() : base(238, 2210333304u, 30, true)
		{
		}
	}
}