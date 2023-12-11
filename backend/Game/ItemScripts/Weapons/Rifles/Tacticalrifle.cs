namespace Game.ItemScripts.Weapons.Rifles
{
	public class Tacticalrifle : WeaponItemScript
	{
		public Tacticalrifle() : base(60, 3520460075u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class TactialrifleAmmo : AmmoItemScript
	{
		public TactialrifleAmmo() : base(119, 3520460075u, 30)
		{
		}
	}
}