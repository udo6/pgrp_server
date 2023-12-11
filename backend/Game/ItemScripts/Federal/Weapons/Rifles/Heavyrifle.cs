namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Heavyrifle : WeaponItemScript
	{
		public Heavyrifle() : base(182, 3347935668u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class HeavyrifleAmmo : AmmoItemScript
	{
		public HeavyrifleAmmo() : base(241, 3347935668u, 30, true)
		{
		}
	}
}