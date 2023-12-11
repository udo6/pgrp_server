namespace Game.ItemScripts.Federal.Weapons.Rifles
{
	public class Assaultrifle : WeaponItemScript
	{
		public Assaultrifle() : base(175, 3220176749u, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.RIFLE)
		{
		}
	}

	public class AssaultrifleAmmo : AmmoItemScript
	{
		public AssaultrifleAmmo() : base(234, 3220176749u, 30, true)
		{
		}
	}
}