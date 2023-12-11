namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Militaryrifle : WeaponItemScript
	{
		public Militaryrifle() : base(183, 2636060646u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class MilitaryrifleAmmo : AmmoItemScript
	{
		public MilitaryrifleAmmo() : base(242, 2636060646u, 30, true)
		{
		}
	}
}