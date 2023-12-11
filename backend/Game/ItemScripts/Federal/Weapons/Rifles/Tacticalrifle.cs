namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Tacticalrifle : WeaponItemScript
	{
		public Tacticalrifle() : base(186, 3520460075u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class TactialrifleAmmo : AmmoItemScript
	{
		public TactialrifleAmmo() : base(245, 3520460075u, 30, true)
		{
		}
	}
}