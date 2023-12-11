namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Bullpuprifle : WeaponItemScript
	{
		public Bullpuprifle() : base(177, 2132975508u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class BullpuprifleAmmo : AmmoItemScript
	{
		public BullpuprifleAmmo() : base(236, 2132975508u, 30, true)
		{
		}
	}
}