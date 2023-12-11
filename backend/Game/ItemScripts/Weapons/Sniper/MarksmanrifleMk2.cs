namespace Game.ItemScripts.Weapons.Sniper
{
	public class MarksmanrifleMk2 : WeaponItemScript
	{
		public MarksmanrifleMk2() : base(90, 1785463520, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SNIPER)
		{
		}
	}

	public class MarksmanrifleMk2Ammo : AmmoItemScript
	{
		public MarksmanrifleMk2Ammo() : base(131, 1785463520, 8)
		{
		}
	}
}