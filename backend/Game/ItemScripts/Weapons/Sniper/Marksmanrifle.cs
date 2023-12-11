namespace Game.ItemScripts.Weapons.Sniper
{
	public class Marksmanrifle : WeaponItemScript
	{
		public Marksmanrifle() : base(89, 3342088282, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class MarksmanrifleAmmo : AmmoItemScript
	{
		public MarksmanrifleAmmo() : base(130, 3342088282, 8)
		{
		}
	}
}