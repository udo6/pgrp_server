namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Compactrifle : WeaponItemScript
	{
		public Compactrifle() : base(181, 1649403952u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class CompactrifleAmmo : AmmoItemScript
	{
		public CompactrifleAmmo() : base(240, 1649403952u, 30, true)
		{
		}
	}
}