namespace Game.ItemScripts.Weapons.Rifles
{
	public class Carbinerifle : WeaponItemScript
	{
		public Carbinerifle() : base(53, 2210333304u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class CarbinerifleAmmo : AmmoItemScript
	{
		public CarbinerifleAmmo() : base(112, 2210333304u, 30)
		{
		}
	}
}