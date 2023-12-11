namespace Game.ItemScripts.Federal.Weapons.Sniper
{
	public class MarksmanrifleMk2 : WeaponItemScript
	{
		public MarksmanrifleMk2() : base(216, 1785463520, true, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class MarksmanrifleMk2Ammo : AmmoItemScript
	{
		public MarksmanrifleMk2Ammo() : base(257, 1785463520, 8, true)
		{
		}
	}
}