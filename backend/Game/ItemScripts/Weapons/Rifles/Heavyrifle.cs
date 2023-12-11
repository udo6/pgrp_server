namespace Game.ItemScripts.Weapons.Rifles
{
	public class Heavyrifle : WeaponItemScript
	{
		public Heavyrifle() : base(56, 3347935668u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class HeavyrifleAmmo : AmmoItemScript
	{
		public HeavyrifleAmmo() : base(115, 3347935668u, 30)
		{
		}
	}
}