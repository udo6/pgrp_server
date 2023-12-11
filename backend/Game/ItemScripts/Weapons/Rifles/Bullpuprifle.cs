namespace Game.ItemScripts.Weapons.Rifles
{
	public class Bullpuprifle : WeaponItemScript
	{
		public Bullpuprifle() : base(51, 2132975508u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class BullpuprifleAmmo : AmmoItemScript
	{
		public BullpuprifleAmmo() : base(110, 2132975508u, 30)
		{
		}
	}
}