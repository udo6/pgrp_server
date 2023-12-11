namespace Game.ItemScripts.Weapons.Rifles
{
	public class Militaryrifle : WeaponItemScript
	{
		public Militaryrifle() : base(57, 2636060646u, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class MilitaryrifleAmmo : AmmoItemScript
	{
		public MilitaryrifleAmmo() : base(116, 2636060646u, 30)
		{
		}
	}
}