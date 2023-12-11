namespace Game.ItemScripts.Weapons.Rifles
{
	public class Compactrifle : WeaponItemScript
	{
		public Compactrifle() : base(55, 1649403952u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class CompactrifleAmmo : AmmoItemScript
	{
		public CompactrifleAmmo() : base(114, 1649403952u, 30)
		{
		}
	}
}