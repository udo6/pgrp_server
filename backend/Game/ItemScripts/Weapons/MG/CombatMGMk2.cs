namespace Game.ItemScripts.Weapons.MG
{
	public class CombatMGMk2 : WeaponItemScript
	{
		public CombatMGMk2() : base(77, 3686625920, false, Core.Enums.InjuryType.SHOT_HIGH, Core.Enums.WeaponType.SMG)
		{
		}
	}

	public class CombatMGMk2Ammo : AmmoItemScript
	{
		public CombatMGMk2Ammo() : base(92, 3686625920, 100)
		{
		}
	}
}