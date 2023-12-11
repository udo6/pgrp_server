namespace Game.ItemScripts.Federal.Weapons.Sniper
{
	public class Marksmanrifle : WeaponItemScript
	{
		public Marksmanrifle() : base(215, 3342088282, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class MarksmanrifleAmmo : AmmoItemScript
	{
		public MarksmanrifleAmmo() : base(256, 3342088282, 8, true)
		{
		}
	}
}